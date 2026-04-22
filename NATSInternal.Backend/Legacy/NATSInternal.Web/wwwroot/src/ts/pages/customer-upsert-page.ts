let pickedIntroducerIdInputElement: HTMLInputElement;
let introducerPanelElement: HTMLDivElement;
let introducerPanelBodyElement: HTMLDivElement;
const panelBodyLoadingClassList = ["opacity-50", "cursor-wait", "pointer-events-none"];

document.addEventListener("DOMContentLoaded", () => {
  if (!document.getElementById("customer-create-page") && !document.getElementById("customer-update-page")) {
    return;
  }

  const upsertFormElement = document.getElementById("customer-upsert-form") as HTMLFormElement;
  const pickedIntroducerIdInputElementId = upsertFormElement
      .getAttribute("data-picked-introducer-id-input-id") as string;
  pickedIntroducerIdInputElement = document.getElementById(pickedIntroducerIdInputElementId) as HTMLInputElement;
  introducerPanelElement = document.getElementById("customer-upsert-introducer-panel") as HTMLDivElement;
  introducerPanelBodyElement = introducerPanelElement.querySelector(".panel-body") as HTMLDivElement;
  addEventListenersToBody();
});

function addEventListenersToBody() {
  addEventListenerToPickedIntroducerBody();
  addEventListenerToCustomerListBody();
}

function addEventListenerToPickedIntroducerBody(): void {
  const unpickIntroducerFormElement = getUnpickIntroducerFormElement();
  unpickIntroducerFormElement && addEventListenerToFormElement(
      unpickIntroducerFormElement,
      () => pickedIntroducerIdInputElement.removeAttribute("value"),
      true);
}

function addEventListenerToCustomerListBody(): void {
  // Customer list form.
  const customerListFormElement = getCustomerListReloadFormElement();
  customerListFormElement && addEventListenerToFormElement(customerListFormElement);
  
  // Pick introducer form.
  const pickIntroducerFormElements = getPickIntroducerFormElements();
  pickIntroducerFormElements.forEach(formElement => {
    addEventListenerToFormElement(
        formElement,
        (formData) => {
          const pickedIntroducerIdName = pickedIntroducerIdInputElement.getAttribute("name") as string;
          const pickedIntroducerId = formData.get(pickedIntroducerIdName) as string 
          if (!pickedIntroducerId) {
            throw new Error("pickedIntroducerId cannot be null here.");
          }

          pickedIntroducerIdInputElement.value = pickedIntroducerId;
        }
    );
  });
}

function addEventListenerToFormElement(
    formElement: HTMLFormElement,
    pickedIntroducerIdInputValueAdjuster?: (submittedData: FormData) => void,
    scrollIntroducerPanelToView?: boolean): void {
  formElement.addEventListener("submit", async (event: SubmitEvent) => {
    event.preventDefault();
    let additionalData: Record<string, string> | undefined = undefined;
    let submitterValue: string | null = null;
    if (event.submitter instanceof HTMLInputElement && event.submitter.hasAttribute("name")) {
      submitterValue = event.submitter.getAttribute("name");
      const name = submitterValue ?? "DefaultName";
      additionalData = { [name]: event.submitter.value };
    }

    const [formData, html] = await fetchPartialViewHTMLAsync(formElement, additionalData);
    introducerPanelBodyElement.innerHTML = html;
    
    if (pickedIntroducerIdInputValueAdjuster) {
      pickedIntroducerIdInputValueAdjuster(formData);
    }
    
    scrollIntroducerPanelToView && introducerPanelBodyElement.scrollIntoView({ behavior: "smooth" });
    addEventListenersToBody();
  });
}

function getCustomerListReloadFormElement(): HTMLFormElement | null {
  return document.getElementById("customer-upsert-customer-list-reload-form") as HTMLFormElement | null;
}

function getUnpickIntroducerFormElement(): HTMLFormElement | null {
  return document.getElementById("customer-upsert-unpick-introducer-form") as HTMLFormElement | null;
}

function getPickIntroducerFormElements(): NodeListOf<HTMLFormElement> {
  return introducerPanelBodyElement.querySelectorAll(".customer-upsert-pick-introducer-form") as NodeListOf<HTMLFormElement>;
}

async function fetchPartialViewHTMLAsync(
    formElement: HTMLFormElement,
    additionalData?: Record<string, string>): Promise<[FormData, string]> {
  panelBodyLoadingClassList.forEach(className => introducerPanelBodyElement.classList.add(className));
  const url = new URL(formElement.action);
  const formData = new FormData(formElement);
  if (additionalData) {
    for (const key in additionalData) {
      formData.set(key, additionalData[key]);
    }
  }

  for (const [key, value] of formData.entries()) {
    url.searchParams.set(key, value as string);
  }
  
  const throttlePromise = new Promise(resolve => setTimeout(resolve, 0));
  const fetchPromise = fetch(url, {
    method: formElement.method,
    body: formElement.method === "post" ? formData : undefined,
    cache: "no-cache"
  });
  
  const [response, _] = await Promise.all([fetchPromise, throttlePromise]);

  panelBodyLoadingClassList.forEach(className => introducerPanelBodyElement.classList.remove(className));
  
  return [formData, await response.text()];
}