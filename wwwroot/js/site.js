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
    // Setup Hero Media Banner: Video with Seamless Transition to Image Carousel
    const heroMediaBanner = document.getElementById("heroMediaBanner");
    if (heroMediaBanner) {
      const heroVideo = document.getElementById("heroVideo");
      const heroVideoWrapper = document.getElementById("heroVideoWrapper");
      const heroSwiperEl = document.getElementById("heroSwiper");
      const heroPrevBtn = document.getElementById("heroPrevBtn");
      const heroNextBtn = document.getElementById("heroNextBtn");
      const heroMediaToggleBtn = document.getElementById("heroMediaToggleBtn");
      const heroMediaToggleIcon = document.getElementById("heroMediaToggleIcon");
      const heroMediaToggleText = document.getElementById("heroMediaToggleText");
      const heroAudioToggleBtn = document.getElementById("heroAudioToggleBtn");
      const heroAudioIcon = document.getElementById("heroAudioIcon");

      let isShowingVideo = true;
      let heroSwiper = null;

      // Initialize Swiper Carousel for the 3 uploaded images
      if (typeof Swiper !== "undefined" && heroSwiperEl) {
        heroSwiper = new Swiper(heroSwiperEl, {
          loop: true,
          speed: 800,
          effect: "fade",
          fadeEffect: { crossFade: true },
          autoplay: {
            delay: 4500,
            disableOnInteraction: false
          },
          pagination: {
            el: "#heroPagination",
            clickable: true
          },
          touchEventsTarget: "container",
          simulateTouch: true,
          grabCursor: true
        });

        // Don't autoplay carousel while video is playing initially
        heroSwiper.autoplay.stop();
      }

      const showCarousel = (slideDirection) => {
        if (heroVideo && !heroVideo.paused) {
          heroVideo.pause();
        }
        if (heroVideoWrapper) {
          heroVideoWrapper.classList.add("hidden");
        }
        isShowingVideo = false;

        if (heroMediaToggleText) heroMediaToggleText.textContent = "Watch Video";
        if (heroMediaToggleIcon) heroMediaToggleIcon.className = "bi bi-play-circle-fill";

        if (heroSwiper) {
          heroSwiper.autoplay.start();
          if (slideDirection === "next") {
            heroSwiper.slideNext();
          } else if (slideDirection === "prev") {
            heroSwiper.slidePrev();
          }
        }
      };

      const showVideo = () => {
        if (heroVideoWrapper) {
          heroVideoWrapper.classList.remove("hidden");
        }
        if (heroVideo) {
          heroVideo.currentTime = 0;
          const playPromise = heroVideo.play();
          if (playPromise !== undefined) {
            playPromise.catch(() => {
              heroVideo.muted = true;
              heroVideo.play();
            });
          }
        }
        isShowingVideo = true;

        if (heroMediaToggleText) heroMediaToggleText.textContent = "View Photos";
        if (heroMediaToggleIcon) heroMediaToggleIcon.className = "bi bi-images";

        if (heroSwiper) {
          heroSwiper.autoplay.stop();
        }
      };

      // 1. When video naturally ends -> seamlessly switch to photo carousel
      if (heroVideo) {
        heroVideo.addEventListener("ended", () => {
          showCarousel();
        });
      }

      // 2. When user clicks anywhere on the video wrapper -> stop video and reveal carousel
      if (heroVideoWrapper) {
        heroVideoWrapper.addEventListener("click", () => {
          showCarousel();
        });
      }

      // 3. Previous / Next Navigation Buttons
      // If clicked while video is playing -> stops video and switches to carousel image
      // If clicked during carousel -> changes the carousel image
      if (heroPrevBtn) {
        heroPrevBtn.addEventListener("click", (e) => {
          e.stopPropagation();
          if (isShowingVideo) {
            showCarousel("prev");
          } else if (heroSwiper) {
            heroSwiper.slidePrev();
          }
        });
      }

      if (heroNextBtn) {
        heroNextBtn.addEventListener("click", (e) => {
          e.stopPropagation();
          if (isShowingVideo) {
            showCarousel("next");
          } else if (heroSwiper) {
            heroSwiper.slideNext();
          }
        });
      }

      // 4. Mode Switch Button (Watch Video <-> View Photos)
      if (heroMediaToggleBtn) {
        heroMediaToggleBtn.addEventListener("click", (e) => {
          e.stopPropagation();
          if (isShowingVideo) {
            showCarousel();
          } else {
            showVideo();
          }
        });
      }

      // 5. Audio Mute / Unmute Button
      if (heroAudioToggleBtn && heroVideo) {
        heroAudioToggleBtn.addEventListener("click", (e) => {
          e.stopPropagation();
          heroVideo.muted = !heroVideo.muted;
          if (heroAudioIcon) {
            heroAudioIcon.className = heroVideo.muted ? "bi bi-volume-mute-fill" : "bi bi-volume-up-fill";
          }
        });
      }
    }
  });
})();
