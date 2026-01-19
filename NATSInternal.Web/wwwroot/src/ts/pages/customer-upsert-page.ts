let panelBody: HTMLDivElement;

document.addEventListener("DOMContentLoaded", () => {
  panelBody = document.querySelector("#customer-upsert-introducer-panel .panel-body") as HTMLDivElement;
  addEventListenersToBody();
});

function addEventListenersToBody() {
  addEventListenerToPickedIntroducerBody();
  addEventListenerToCustomerListBody();
}

function addEventListenerToPickedIntroducerBody(): void {
  addEventListenerToFormElementIfNotNull(getUnpickedIntroducerFormElement());
}

function addEventListenerToCustomerListBody(): void {
  // Customer list.
  const customerListFormElement = getCustomerListReloadFormElement();
  customerListFormElement?.addEventListener("submit", async (event: SubmitEvent) => {
    event.preventDefault();
    let additionalData: Record<string, string> | undefined = undefined;
    if (event.submitter instanceof HTMLInputElement && event.submitter.hasAttribute("name")) {
      const name = event.submitter.getAttribute("name") ?? "DefaultName";
      additionalData = { [name]: event.submitter.value }
    }

    const classList = ["opacity-50", "cursor-wait", "pointer-events-none"];
    classList.forEach(className => panelBody.classList.add(className));
    panelBody.innerHTML = await fetchPartialViewHTMLAsync(customerListFormElement, additionalData);
    classList.forEach(className => panelBody.classList.remove(className));
    addEventListenersToBody();
  });
  
  addEventListenerToFormElementIfNotNull(getPickIntroducerFormElement());
}

function addEventListenerToFormElementIfNotNull(formElement: HTMLFormElement | null): void {
  formElement?.addEventListener("submit", async (event: SubmitEvent) => {
    event.preventDefault();
    const classList = ["opacity-50", "cursor-wait", "pointer-events-none"];
    classList.forEach(className => panelBody.classList.add(className));
    panelBody.innerHTML = await fetchPartialViewHTMLAsync(formElement);
    classList.forEach(className => panelBody.classList.remove(className));
    addEventListenersToBody();
  });
}

function getCustomerListReloadFormElement(): HTMLFormElement | null {
  return document.getElementById("customer-upsert-customer-list-reload-form") as HTMLFormElement | null;
}

function getUnpickedIntroducerFormElement(): HTMLFormElement | null {
  return document.getElementById("customer-upsert-unpick-introducer-form") as HTMLFormElement | null;
}

function getPickIntroducerFormElement(): HTMLFormElement | null {
  return document.getElementById("customer-upsert-pick-introducer-form") as HTMLFormElement | null;
}

async function fetchPartialViewHTMLAsync(
    formElement: HTMLFormElement,
    additionalData?: Record<string, string>): Promise<string> {
  const url = new URL(formElement.action);
  const formData = new FormData(formElement);
  if (additionalData) {
    for (const key in additionalData) {
      formData.set(key, additionalData[key]);
    }
  }
  
  if (formElement.method === "get") {
    for (const [key, value] of formData.entries()) {
      url.searchParams.set(key, value as string);
    }
  }
  
  await new Promise(resolve => setTimeout(resolve, 300));
  
  const response = await fetch(url, {
    method: formElement.method,
    body: formElement.method === "post" ? formData : undefined,
    cache: "no-cache"
  });
  
  return await response.text();
}