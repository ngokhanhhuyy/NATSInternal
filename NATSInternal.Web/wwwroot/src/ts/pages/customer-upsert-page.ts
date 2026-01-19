let panelBody: HTMLDivElement;
const panelBodyLoadingClassList = ["opacity-50", "cursor-wait", "pointer-events-none"];

document.addEventListener("DOMContentLoaded", () => {
  panelBody = document.querySelector("#customer-upsert-introducer-panel .panel-body") as HTMLDivElement;
  addEventListenersToBody();
});

function addEventListenersToBody() {
  addEventListenerToPickedIntroducerBody();
  addEventListenerToCustomerListBody();
}

function addEventListenerToPickedIntroducerBody(): void {
  const unpickIntroducerFormElement = getUnpickIntroducerFormElement();
  unpickIntroducerFormElement?.addEventListener("submit", async (event: SubmitEvent) => {
    event.preventDefault();
    panelBody.innerHTML = await fetchPartialViewHTMLAsync(unpickIntroducerFormElement);
    panelBody.scrollIntoView({ behavior: "smooth" });
    addEventListenersToBody();
  });
}

function addEventListenerToCustomerListBody(): void {
  // Customer list form.
  const customerListFormElement = getCustomerListReloadFormElement();
  customerListFormElement?.addEventListener("submit", async (event: SubmitEvent) => {
    event.preventDefault();
    let additionalData: Record<string, string> | undefined = undefined;
    if (event.submitter instanceof HTMLInputElement && event.submitter.hasAttribute("name")) {
      const name = event.submitter.getAttribute("name") ?? "DefaultName";
      additionalData = { [name]: event.submitter.value }
    }

    panelBody.innerHTML = await fetchPartialViewHTMLAsync(customerListFormElement, additionalData);
    addEventListenersToBody();
  });
  
  // Pick introducer form.
  const pickIntroducerFormElements = getPickIntroducerFormElements();
  pickIntroducerFormElements.forEach(formElement => {
    formElement.addEventListener("submit", async (event: SubmitEvent) => {
      event.preventDefault();
      let additionalData: Record<string, string> | undefined = undefined;
      if (event.submitter instanceof HTMLInputElement && event.submitter.hasAttribute("name")) {
        const name = event.submitter.getAttribute("name") ?? "DefaultName";
        additionalData = {[name]: event.submitter.value}
      }

      panelBody.innerHTML = await fetchPartialViewHTMLAsync(formElement, additionalData);
      addEventListenersToBody();
    });
  });
}

function getCustomerListReloadFormElement(): HTMLFormElement | null {
  return document.getElementById("customer-upsert-customer-list-reload-form") as HTMLFormElement | null;
}

function getUnpickIntroducerFormElement(): HTMLFormElement | null {
  return document.getElementById("customer-upsert-unpick-introducer-form") as HTMLFormElement | null;
}

function getPickIntroducerFormElements(): NodeListOf<HTMLFormElement> {
  return panelBody.querySelectorAll(".customer-upsert-pick-introducer-form") as NodeListOf<HTMLFormElement>;
}

async function fetchPartialViewHTMLAsync(
    formElement: HTMLFormElement,
    additionalData?: Record<string, string>): Promise<string> {
  panelBodyLoadingClassList.forEach(className => panelBody.classList.add(className));
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
  
  await new Promise(resolve => setTimeout(resolve, 300));
  
  const response = await fetch(url, {
    method: formElement.method,
    body: formElement.method === "post" ? formData : undefined,
    cache: "no-cache"
  });

  panelBodyLoadingClassList.forEach(className => panelBody.classList.remove(className));
  
  return await response.text();
}