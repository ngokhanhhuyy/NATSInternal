document.addEventListener("DOMContentLoaded", () => {
  const modalElement = handleIntroducerModal();
  handleCustomerListForm();
});

function handleIntroducerModal(): HTMLDivElement | null {
  const modalOpenClassName = "modal-open";
  const togglingButtonElement = document.getElementById("introducer-modal-toggling-button");
  if (!togglingButtonElement) {
    return null;
  }
  
  const modalElement = document.getElementById("customer-introducer-picker-modal") as HTMLDivElement | null;
  if (!modalElement) {
    return null;
  }
  
  const toggleModal = () => {
    if (modalElement.classList.contains(modalOpenClassName)) {
      modalElement.classList.remove(modalOpenClassName);
    } else {
      modalElement.classList.add(modalOpenClassName);
    }
  };

  togglingButtonElement.addEventListener("click", toggleModal);
  
  const modalCancelButtonElement = document.getElementById("customer-introducer-picker-modal-cancel-button");
  if (!modalCancelButtonElement) {
    return modalElement;
  }

  modalCancelButtonElement.addEventListener("click", () => modalElement.classList.remove(modalOpenClassName));
  modalElement.addEventListener("mousedown", (event: MouseEvent) => {
    if (event.target != modalElement) {
      return;
    }
    
    modalElement.classList.remove(modalOpenClassName)
  });
  
  return modalElement;
}

function handleCustomerListForm(): void {
  const formElement = document.getElementById("customer-upsert-customer-list-form") as HTMLFormElement | null;
  if (!formElement) {
    return;
  }

  const listResultsContainerElement = formElement.querySelector(".results-list-container") as HTMLDivElement | null;
  if (!listResultsContainerElement) {
    return;
  }
  
  formElement.addEventListener("submit", async (event: SubmitEvent) => {
    event.preventDefault();
    const action = formElement.getAttribute("action") ?? "/";

    const url = new URL(action, window.location.origin);
    const formData = new FormData(formElement);
    
    for (const [key, value] of formData.entries()) {
      url.searchParams.append(key, value as string);
    }
    
    const submitter = event.submitter as HTMLInputElement | null;
    if (submitter && submitter.name) {
      url.searchParams.append(submitter.name, submitter.value);
    }
    
    const reloadingClassNames = ["opacity-50", "pointer-events-none"];
    reloadingClassNames.forEach(name => formElement.classList.add(name));
    
    const response = await fetch(url);
    const html = await response.text();
    listResultsContainerElement.innerHTML = html.trim();

    reloadingClassNames.forEach(name => formElement.classList.remove(name));
  });
}

function handleIntroducerPickButtons(modalElement: HTMLDivElement): void {
  const pickButtonElements = modalElement.querySelectorAll(".introducer-pick-button") as NodeListOf<HTMLButtonElement>;
  pickButtonElements.forEach(buttonElement => {
    const introducerId = buttonElement.getAttribute("data-introducer-id") as string;
    buttonElement.addEventListener("click", () => {
      
    });
  });
}