/**
 * Dynamic Real Estate Portfolio Platform - Client Application Script
 */

(function () {
  "use strict";

  // 1. Theme Management (Bootstrap 5.3 data-bs-theme)
  const getStoredTheme = () => localStorage.getItem("theme");
  const setStoredTheme = (theme) => localStorage.setItem("theme", theme);

  const getPreferredTheme = () => {
    const storedTheme = getStoredTheme();
    if (storedTheme) return storedTheme;
    return window.matchMedia("(prefers-color-scheme: dark)").matches ? "dark" : "light";
  };

  const setTheme = (theme) => {
    document.documentElement.setAttribute("data-bs-theme", theme);
    const themeIcon = document.getElementById("themeIcon");
    const themeText = document.getElementById("themeText");
    if (themeIcon) {
      themeIcon.className = theme === "dark" ? "bi bi-moon-stars-fill" : "bi bi-sun-fill";
    }
    if (themeText) {
      themeText.textContent = theme === "dark" ? "Dark Mode" : "Light Mode";
    }
  };

  setTheme(getPreferredTheme());

  document.addEventListener("DOMContentLoaded", () => {
    // Theme Toggle Button
    const themeToggleBtn = document.getElementById("themeToggleBtn");
    if (themeToggleBtn) {
      themeToggleBtn.addEventListener("click", () => {
        const currentTheme = document.documentElement.getAttribute("data-bs-theme");
        const newTheme = currentTheme === "dark" ? "light" : "dark";
        setStoredTheme(newTheme);
        setTheme(newTheme);
      });
    }

    // Initialize AOS (Animate On Scroll)
    if (typeof AOS !== "undefined") {
      AOS.init({
        duration: 750,
        easing: "ease-out-cubic",
        once: true,
        offset: 60
      });
    }

    // Setup AJAX Inquiry Modals
    const ajaxInquiryForms = document.querySelectorAll(".ajax-inquiry-form");
    ajaxInquiryForms.forEach((form) => {
      form.addEventListener("submit", async (e) => {
        e.preventDefault();
        const submitBtn = form.querySelector('button[type="submit"]');
        const alertBox = form.querySelector(".inquiry-alert-box");

        const payload = {
          clientName: form.querySelector('[name="ClientName"]').value,
          emailAddress: form.querySelector('[name="EmailAddress"]').value,
          phoneNumber: form.querySelector('[name="PhoneNumber"]')?.value || "",
          subject: form.querySelector('[name="Subject"]').value,
          message: form.querySelector('[name="Message"]').value,
          preferredProjectSlug: form.querySelector('[name="PreferredProjectSlug"]')?.value || ""
        };

        if (submitBtn) {
          submitBtn.disabled = true;
          submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Submitting...';
        }

        try {
          const response = await fetch("/Home/SubmitLeadAjax", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
          });
          const result = await response.json();

          if (alertBox) {
            alertBox.className = "alert alert-success mt-3";
            alertBox.textContent = result.message || "Inquiry received successfully!";
            alertBox.classList.remove("d-none");
          }
          form.reset();
        } catch (err) {
          if (alertBox) {
            alertBox.className = "alert alert-danger mt-3";
            alertBox.textContent = "Failed to transmit inquiry. Please try again or reach out directly.";
            alertBox.classList.remove("d-none");
          }
        } finally {
          if (submitBtn) {
            submitBtn.disabled = false;
            submitBtn.innerHTML = '<i class="bi bi-send-fill me-2"></i>Send VIP Inquiry';
          }
        }
      });
    });
  });
})();
