namespace NATSInternal.Services;

/// <inheritdoc cref="ITreatmentService" />
internal class TreatmentService : LockableEntityService, ITreatmentService
{
    private readonly DatabaseContext _context;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly ITreatmentPhotoService _photoService;
    private readonly IStatsInternalService<Treatment, User, TreatmentUpdateHistory> _statsService;
    private readonly IProductEngagementService<TreatmentItem, Product, TreatmentPhoto, User, TreatmentUpdateHistory> _productEngagementService;
    private readonly IUpdateHistoryService<Treatment, User, TreatmentUpdateHistory, TreatmentUpdateHistoryDataDto> _updateHistoryService;
    private readonly IMonthYearService<Treatment, User, TreatmentUpdateHistory> _monthYearService;

    public TreatmentService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService,
            ITreatmentPhotoService photoService,
            IStatsInternalService<Treatment, User, TreatmentUpdateHistory> statsService,
            IProductEngagementService<TreatmentItem, Product, TreatmentPhoto, User, TreatmentUpdateHistory> productEngagementService,
            IUpdateHistoryService<Treatment, User, TreatmentUpdateHistory, TreatmentUpdateHistoryDataDto> updateHistoryService,
            IMonthYearService<Treatment, User, TreatmentUpdateHistory> monthYearService)
    {
        _context = context;
        _authorizationService = authorizationService;
        _photoService = photoService;
        _statsService = statsService;
        _productEngagementService = productEngagementService;
        _updateHistoryService = updateHistoryService;
        _monthYearService = monthYearService;
    }

    /// <inheritdoc />
    public async Task<TreatmentListResponseDto> GetListAsync(
            TreatmentListRequestDto requestDto)
    {
        // Initialize list of month and year options.
        List<MonthYearResponseDto> monthYearOptions = await _monthYearService
            .GenerateMonthYearOptions(dbContext => dbContext.Treatments);

        // Initialize query.
        IQueryable<Treatment> query = _context.Treatments
            .Include(t => t.Customer)
            .Include(t => t.Items)
            .Include(t => t.Photos);

        // Sorting by direction and sorting by field filter.
        Expression<Func<Treatment, long>> amountExpression = (t) => t.Items.Sum(ti =>
            (ti.AmountBeforeVatPerUnit + ti.VatAmountPerUnit) * ti.Quantity) +
            t.ServiceAmountBeforeVat + t.ServiceAmountBeforeVat;
        switch (requestDto.OrderByField)
        {
            case nameof(TreatmentListRequestDto.FieldOptions.Amount):
                query = requestDto.OrderByAscending
                    ? query.OrderBy(Treatment.AmountAfterVatExpression)
                        .ThenBy(t => t.PaidDateTime)
                    : query.OrderByDescending(Treatment.AmountAfterVatExpression)
                        .ThenByDescending(t => t.PaidDateTime);
                break;
            case nameof(TreatmentListRequestDto.FieldOptions.PaidDateTime):
                query = requestDto.OrderByAscending
                    ? query.OrderBy(t => t.PaidDateTime)
                        .ThenBy(Treatment.AmountAfterVatExpression)
                    : query.OrderByDescending(t => t.PaidDateTime)
                        .ThenBy(Treatment.AmountAfterVatExpression);
                break;
        }

        // Filter by month and year if specified.
        if (!requestDto.IgnoreMonthYear)
        {
            DateTime startDateTime = new DateTime(requestDto.Year, requestDto.Month, 1);
            DateTime endDateTime = startDateTime.AddMonths(1);
            query = query
                .Where(s => s.PaidDateTime >= startDateTime && s.PaidDateTime < endDateTime);
        }

        // Filter by user id if specified.
        if (requestDto.UserId.HasValue)
        {
            query = query.Where(t => t.CreatedUserId == requestDto.UserId);
        }

        // Filter by customer id if specified.
        if (requestDto.CustomerId.HasValue)
        {
            query = query.Where(c => c.CustomerId == requestDto.CustomerId);
        }

        // Filter by product id if specified.
        if (requestDto.ProductId.HasValue)
        {
            query = query.Where(t => t.Items.Any(oi => oi.ProductId == requestDto.ProductId));
        }

        // Filter by not being soft deleted.
        query = query.Where(o => !o.IsDeleted);

        // Initialize response dto.
        TreatmentListResponseDto responseDto = new TreatmentListResponseDto
        {
            MonthYearOptions = monthYearOptions,
            Authorization = _authorizationService.GetTreatmentListAuthorization()
        };
        int resultCount = await query.CountAsync();
        if (resultCount == 0)
        {
            responseDto.PageCount = 0;
            return responseDto;
        }

        responseDto.PageCount = (int)Math.Ceiling(
            (double)resultCount / requestDto.ResultsPerPage);
        responseDto.Items = await query
            .Select(t => new TreatmentBasicResponseDto(
                t,
                _authorizationService.GetTreatmentAuthorization(t)))
            .Skip(requestDto.ResultsPerPage * (requestDto.Page - 1))
            .Take(requestDto.ResultsPerPage)
            .AsSplitQuery()
            .ToListAsync();

        return responseDto;
    }

    /// <inheritdoc />
    public async Task<TreatmentDetailResponseDto> GetDetailAsync(int id)
    {
        // Initialize query.
        IQueryable<Treatment> query = _context.Treatments
            .Include(t => t.Customer)
            .Include(t => t.Items)
            .Include(t => t.Photos);

        // Determine if the update histories should be fetched.
        bool shouldIncludeUpdateHistories = _authorizationService
            .CanAccessTreatmentUpdateHistories();
        if (shouldIncludeUpdateHistories)
        {
            query = query.Include(t => t.UpdateHistories);
        }

        // Fetch the entity with the given id and ensure it exists in the database.
        Treatment treatment = await query
            .AsSplitQuery()
            .SingleOrDefaultAsync(t => t.Id == id && !t.IsDeleted)
            ?? throw new ResourceNotFoundException(
                nameof(Treatment),
                nameof(id),
                id.ToString());

        return new TreatmentDetailResponseDto(
            treatment,
            _authorizationService.GetTreatmentAuthorization(treatment),
            mapUpdateHistories: shouldIncludeUpdateHistories);
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(TreatmentUpsertRequestDto requestDto)
    {
        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        // Determine SupplyDateTime.
        DateTime paidDateTime = DateTime.UtcNow.ToApplicationTime();
        if (requestDto.PaidDateTime.HasValue)
        {
            // Check if the currentuser has permission to specify the treatmented datetime.
            if (!_authorizationService.CanSetTreatmentPaidDateTime())
            {
                throw new AuthorizationException();
            }

            paidDateTime = requestDto.PaidDateTime.Value;
        }

        // Initialize treatment entity.
        Treatment treatment = new Treatment
        {
            PaidDateTime = paidDateTime,
            CreatedDateTime = DateTime.UtcNow.ToApplicationTime(),
            Note = requestDto.Note,
            CreatedUserId = _authorizationService.GetUserId(),
            CustomerId = requestDto.CustomerId,
        };

        // Initialize treatment item entites.
        await _productEngagementService
            .CreateItemsAsync(treatment.Items, requestDto.Items, ProductEngagementType.Export);

        // Initialize photos.
        if (requestDto.Photos != null)
        {
            await _photoService.CreateMultipleAsync(treatment, requestDto.Photos);
        }

        // Perform the creating operation.
        try
        {
            await _context.SaveChangesAsync();

            // The treatment can be created successfully without any error. Add the treatment
            // to the stats.
            DateOnly treatmentedDate = DateOnly.FromDateTime(treatment.PaidDateTime);
            await _statsService.IncrementTreatmentGrossRevenueAsync(treatment.AmountBeforeVat, treatmentedDate);
            if (treatment.ProductVatAmount > 0)
            {
                await _statsService.IncrementVatCollectedAmountAsync(treatment.ProductVatAmount, treatmentedDate);
            }

            // Commit the transaction, finish the operation.
            await transaction.CommitAsync();
            return treatment.Id;
        }
        catch (DbUpdateException exception)
        {
            // Remove all the created photos.
            foreach (TreatmentPhoto photo in treatment.Photos)
            {
                _photoService.Delete(photo.Url);
            }

            // Handle the concurency exception.
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }

            // Handle the operation exception and convert to the appropriate error.
            if (exception.InnerException is MySqlException sqlException)
            {
                SqlExceptionHandler exceptionHandler = new SqlExceptionHandler();
                exceptionHandler.Handle(sqlException);
                if (exceptionHandler.IsForeignKeyNotFound)
                {
                    string errorMessage;
                    switch (exceptionHandler.ViolatedFieldName)
                    {
                        case "customer_id":
                            errorMessage = ErrorMessages.NotFound
                                .ReplaceResourceName(DisplayNames.Customer);
                            break;
                        case "cread_user_id" or "therapist_id":
                            errorMessage = ErrorMessages.NotFound
                                .ReplaceResourceName(DisplayNames.User);
                            break;
                        default:
                            errorMessage = ErrorMessages.Undefined;
                            break;
                    }
                    throw new OperationException(errorMessage);
                }
            }
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, TreatmentUpsertRequestDto requestDto)
    {
        // Fetch the entity from the database and ensure it exists.
        Treatment treatment = await _context.Treatments
            .Include(t => t.CreatedUser)
            .Include(t => t.Therapist)
            .Include(t => t.Items).ThenInclude(ti => ti.Product)
            .Include(t => t.Photos)
            .SingleOrDefaultAsync(t => t.Id == id && !t.IsDeleted)
            ?? throw new ResourceNotFoundException(
                nameof(Treatment),
                nameof(id),
                id.ToString());

        // Check if the current user has permission to edit the treatment.
        if (!_authorizationService.CanEditTreatment(treatment))
        {
            throw new AuthorizationException();
        }
        
        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();
        
        // Storing the old data for update history logging and stats adjustment.
        long oldAmount = treatment.AmountBeforeVat;
        long oldVatAmount = treatment.ProductVatAmount;
        DateOnly oldPaidDate = DateOnly.FromDateTime(treatment.PaidDateTime);
        TreatmentUpdateHistoryDataDto oldData = new TreatmentUpdateHistoryDataDto(treatment);

        // Handle the new treatmented datetime when the request specifies it.
        if (requestDto.PaidDateTime.HasValue)
        {
            // Check if the current user has permission to specify a new treatmented datetime.
            if (!_authorizationService.CanSetTreatmentPaidDateTime())
            {
                throw new AuthorizationException();
            }

            // Prevent the treatment's SupplyDateTime to be modified when the treatment is
            // locked.
            if (treatment.IsLocked)
            {
                string errorMessage = ErrorMessages.CannotSetDateTimeAfterLocked
                    .ReplaceResourceName(DisplayNames.Treatment)
                    .ReplacePropertyName(DisplayNames.PaidDateTime);
                throw new OperationException(
                    nameof(requestDto.PaidDateTime),
                    errorMessage);
            }

            // Assign the new SupplyDateTime value only if it's different from the old one.
            if (requestDto.PaidDateTime.Value != treatment.PaidDateTime)
            {
                // Validate and assign the specified SupplyDateTime value from the request.
                try
                {
                    _statsService.ValidateStatsDateTime(
                        treatment,
                        requestDto.PaidDateTime.Value);
                    treatment.PaidDateTime = requestDto.PaidDateTime.Value;
                }
                catch (ArgumentException exception)
                {
                    string errorMessage = exception.Message
                        .ReplacePropertyName(DisplayNames.PaidDateTime);
                    throw new OperationException(
                        nameof(requestDto.PaidDateTime),
                        errorMessage);
                }
            }
        }

        // Update treatment properties.
        treatment.Note = requestDto.Note;
        treatment.TherapistId = requestDto.TherapistId;

        // Update treatment items.
        await _productEngagementService.UpdateItemsAsync(
            treatment.Items,
            requestDto.Items,
            ProductEngagementType.Export,
            DisplayNames.TreatmentItem);

        // Update photos.
        List<string> urlsToBeDeletedWhenSucceeds = new List<string>();
        List<string> urlsToBeDeletedWhenFails = new List<string>();
        if (requestDto.Photos != null)
        {
            (List<string>, List<string>) photoUpdateResults;
            photoUpdateResults = await _photoService.UpdateMultipleAsync(
                treatment,
                requestDto.Photos);
            urlsToBeDeletedWhenSucceeds.AddRange(photoUpdateResults.Item1);
            urlsToBeDeletedWhenFails.AddRange(photoUpdateResults.Item2);
        }
        
        // Store new data for update history logging.
        TreatmentUpdateHistoryDataDto newData = new TreatmentUpdateHistoryDataDto(treatment);
        
        // Log update history.
        _updateHistoryService.LogUpdateHistory(treatment, oldData, newData, requestDto.UpdateReason);

        // Save changes and handle the errors.
        try
        {
            // Save all modifications.
            await _context.SaveChangesAsync();

            // The treatment can be updated successfully without any error.
            // Adjust the stats for items' amount and vat collected amount.
            // Revert the old stats.
            await _statsService.IncrementTreatmentGrossRevenueAsync(-oldAmount, oldPaidDate);
            await _statsService.IncrementVatCollectedAmountAsync(-oldVatAmount, oldPaidDate);
            
            // Create new stats.
            DateOnly newPaidDate = DateOnly.FromDateTime(treatment.PaidDateTime);
            await _statsService.IncrementRetailGrossRevenueAsync(treatment.AmountBeforeVat, newPaidDate);
            await _statsService.IncrementVatCollectedAmountAsync(treatment.ProductVatAmount, newPaidDate);

            // Delete all old photos which have been replaced by new ones.
            foreach (string url in urlsToBeDeletedWhenSucceeds)
            {
                _photoService.Delete(url);
            }
            
            // Commit the transaction and finish the operation.
            await transaction.CommitAsync();
        }
        catch (DbUpdateException exception)
        {
            // Undo all the created photos.
            foreach (string url in urlsToBeDeletedWhenFails)
            {
                _photoService.Delete(url);
            }

            // Handle concurrency exception.
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }

            // Handling the exception in the foreseen cases.
            if (exception.InnerException is MySqlException sqlException)
            {
                SqlExceptionHandler exceptionHandler = new SqlExceptionHandler();
                exceptionHandler.Handle(sqlException);
                if (exceptionHandler.IsForeignKeyNotFound)
                {
                    switch (exceptionHandler.ViolatedFieldName)
                    {
                        case "customer_id":
                            throw new ResourceNotFoundException(
                                nameof(Customer),
                                nameof(requestDto.CustomerId),
                                requestDto.CustomerId.ToString());
                        case "therapist_id":
                            throw new ResourceNotFoundException(
                                nameof(User),
                                nameof(requestDto.TherapistId),
                                requestDto.TherapistId.ToString());
                        case "created_user_id":
                            throw new ConcurrencyException();
                    }
                }
            }
            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        // Fetch the entity from the database and ensure it exists.
        Treatment treatment = await _context.Treatments
            .Include(t => t.Items).ThenInclude(i => i.Product).ThenInclude(p => p.Photos)
            .Include(t => t.Photos)
            .SingleOrDefaultAsync(t => t.Id == id && !t.IsDeleted)
            ?? throw new ResourceNotFoundException(
                nameof(Treatment),
                nameof(id),
                id.ToString());

        // Check if the current user has permission to delete the order.
        if (!_authorizationService.CanDeleteTreatment())
        {
            throw new AuthorizationException();
        }

        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        // Delete all the items associated to this treatment.
        _productEngagementService.DeleteItems(
            treatment.Items,
            dbContext => dbContext.TreatmentItems,
            ProductEngagementType.Export);

        // Delete the treatment entity.
        _context.Treatments.Remove(treatment);

        // Perform the deleting operation.
        try
        {
            await _context.SaveChangesAsync();
            
            // The treatment can be deleted successfully without any error.
            // Revert the stats associated to the treatment.
            DateOnly paidDate = DateOnly.FromDateTime(treatment.PaidDateTime);
            await _statsService.IncrementTreatmentGrossRevenueAsync(-treatment.AmountBeforeVat, paidDate);
            await _statsService.IncrementVatCollectedAmountAsync(-treatment.ProductVatAmount, paidDate);

            // Commit the transaction and finish the operation.
            await transaction.CommitAsync();

            // Deleted all the created photo files for the supply.
            foreach (string url in treatment.Photos.Select(p => p.Url).ToList())
            {
                _photoService.Delete(url);
            }
        }
        catch (DbUpdateException exception)
        {
            // Handle the concurrency exception.
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }

            // Handle operation exception.
            if (exception.InnerException is MySqlException sqlException)
            {
                SqlExceptionHandler exceptionHandler = new SqlExceptionHandler();
                exceptionHandler.Handle(sqlException);
                if (exceptionHandler.IsDeleteOrUpdateRestricted)
                {
                    // Soft delete when there are any other related entities which
                    // are restricted to be deleted.
                    treatment.IsDeleted = true;

                    // Save changes.
                    await _context.SaveChangesAsync();

                    // Order has been deleted successfully, adjust the stats.
                    DateOnly orderedDate = DateOnly.FromDateTime(treatment.PaidDateTime);
                    await _statsService.IncrementRetailGrossRevenueAsync(
                        treatment.AmountBeforeVat,
                        orderedDate);
                    await _statsService.IncrementVatCollectedAmountAsync(
                        treatment.ProductVatAmount,
                        orderedDate);

                    // Commit the transaction and finishing the operations.
                    await transaction.CommitAsync();
                }
            }
            throw;
        }
    }
}