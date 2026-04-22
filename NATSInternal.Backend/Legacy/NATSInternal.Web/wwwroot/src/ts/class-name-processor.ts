document.addEventListener("DOMContentLoaded", () => {
  document.querySelectorAll("[class]").forEach(element => {
    const classNames = element.className;
    const splittedClassNames = classNames.split(/\s+/)
    const distinctClassNames: string[] = []
    splittedClassNames.forEach(name => {
      if (!distinctClassNames.includes(name)) {
        distinctClassNames.push(name);
      }
    });
    
    element.className = distinctClassNames.join(" ");
  });
})