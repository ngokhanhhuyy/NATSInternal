document.addEventListener("DOMContentLoaded", () => {
  const remainingSecondsElement: HTMLSpanElement | null = document.querySelector("#error-page #remaining-seconds");
  if (!remainingSecondsElement) {
    return;
  }

  const returningUrl = remainingSecondsElement.getAttribute("data-returning-url");
  if (!returningUrl) {
    return;
  }
  
  let remainingSeconds = parseInt(remainingSecondsElement.innerText);
  setInterval(() => {
    remainingSeconds -= 1;
    remainingSecondsElement.innerText = remainingSeconds.toString();
  }, 1000);
});