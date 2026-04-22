document.addEventListener("DOMContentLoaded", () => {
  if (!document.getElementById("confirmation-page")) {
    return;
  }

  const remainingSecondsElement: HTMLSpanElement | null =
    document.querySelector("#confirmation-page #remaining-seconds");

  if (!remainingSecondsElement) {
    return;
  }

  const returningButtonElement = document.getElementById("returning-button");
  let returningUrl = returningButtonElement?.getAttribute("href") ?? "";
  if (!returningButtonElement || !returningUrl) {
    returningUrl = "/";
  }
  
  let remainingSeconds = parseInt(remainingSecondsElement.innerText);
  const interval = setInterval(() => {
    remainingSeconds -= 1;
    remainingSecondsElement.innerText = remainingSeconds.toString();
    
    if (remainingSeconds == 0) {
      clearInterval(interval);
      window.location.href = returningUrl;
    }
  }, 1000);
});