/**
 * Dynamic Real Estate Portfolio Platform - Client Application Script
 */

(function () {
  "use strict";

  // 1. Theme Management (Default clean light theme)
  const getStoredTheme = () => localStorage.getItem("theme");
  const setStoredTheme = (theme) => localStorage.setItem("theme", theme);

  const getPreferredTheme = () => {
    const storedTheme = getStoredTheme();
    if (storedTheme) return storedTheme;
    return "light";
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
    // 2. Navbar elevation on scroll
    const navbar = document.querySelector('.navbar');
    if (navbar) {
      const handleNavScroll = () => {
        if (window.scrollY > 30) {
          navbar.classList.add('navbar-scrolled');
        } else {
          navbar.classList.remove('navbar-scrolled');
        }
      };
      window.addEventListener('scroll', handleNavScroll, { passive: true });
      handleNavScroll();
    }

    // Theme Toggle Button (if present)
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
        offset: 50
      });
    }

    // Setup AJAX Inquiry Modals & Forms
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
            alertBox.textContent = result.message || "Thank you! Your inquiry has been received.";
            alertBox.classList.remove("d-none");
          }
          form.reset();
        } catch (err) {
          if (alertBox) {
            alertBox.className = "alert alert-danger mt-3";
            alertBox.textContent = "Failed to transmit inquiry. Please reach out to our office directly.";
            alertBox.classList.remove("d-none");
          }
        } finally {
          if (submitBtn) {
            submitBtn.disabled = false;
            submitBtn.innerHTML = '<i class="bi bi-send-fill me-2"></i>Send Inquiry';
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
      const heroAudioToggleBtn = document.getElementById("heroAudioToggleBtn");
      const heroAudioIcon = document.getElementById("heroAudioIcon");
      const heroAudioText = document.getElementById("heroAudioText");

      let isShowingVideo = true;
      let heroSwiper = null;

      const updateAudioUI = () => {
        if (!heroAudioIcon) return;
        if (heroVideo && heroVideo.muted) {
          heroAudioIcon.className = "bi bi-volume-mute-fill";
          if (heroAudioText) heroAudioText.textContent = "Sound Off";
          if (heroAudioToggleBtn) heroAudioToggleBtn.setAttribute("title", "Turn Sound On");
        } else {
          heroAudioIcon.className = "bi bi-volume-up-fill";
          if (heroAudioText) heroAudioText.textContent = "Sound On";
          if (heroAudioToggleBtn) heroAudioToggleBtn.setAttribute("title", "Turn Sound Off");
        }
      };

      // Video Music Autoplay First
      if (heroVideo) {
        heroVideo.muted = false;
        heroVideo.volume = 1.0;
        updateAudioUI();

        const playPromise = heroVideo.play();
        if (playPromise !== undefined) {
          playPromise.catch(() => {
            // Browser restricted unmuted autoplay: start muted and immediately unmute on first gesture
            heroVideo.muted = true;
            updateAudioUI();
            heroVideo.play().catch(() => {});

            const autoUnmuteOnGesture = () => {
              if (heroVideo) {
                heroVideo.muted = false;
                heroVideo.volume = 1.0;
                updateAudioUI();
              }
              ['pointerdown', 'touchstart', 'click', 'scroll', 'keydown'].forEach(evt => {
                window.removeEventListener(evt, autoUnmuteOnGesture);
              });
            };

            ['pointerdown', 'touchstart', 'click', 'scroll', 'keydown'].forEach(evt => {
              window.addEventListener(evt, autoUnmuteOnGesture, { once: true, passive: true });
            });
          });
        }
      }

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
              updateAudioUI();
              heroVideo.play().catch(() => {});
            });
          }
        }
        isShowingVideo = true;

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

      // 4. Audio Mute / Unmute Button
      if (heroAudioToggleBtn && heroVideo) {
        heroAudioToggleBtn.addEventListener("click", (e) => {
          e.stopPropagation();
          heroVideo.muted = !heroVideo.muted;
          if (!heroVideo.muted && heroVideo.paused && isShowingVideo) {
            heroVideo.play().catch(() => {});
          }
          updateAudioUI();
        });
      }
    }

    // 5. Setup Gallery Grid Lightbox Modal
    const galleryTiles = document.querySelectorAll(".gallery-tile");
    const lightboxModalEl = document.getElementById("galleryLightboxModal");
    if (galleryTiles.length > 0 && lightboxModalEl && typeof bootstrap !== "undefined") {
      const lightboxModal = new bootstrap.Modal(lightboxModalEl);
      const modalImg = document.getElementById("lightboxModalImg");
      const modalTitle = document.getElementById("lightboxModalTitle");
      const modalCategory = document.getElementById("lightboxModalCategory");

      galleryTiles.forEach((tile) => {
        tile.addEventListener("click", () => {
          const imgSrc = tile.getAttribute("data-img");
          const title = tile.getAttribute("data-title");
          const category = tile.getAttribute("data-category");

          if (modalImg) modalImg.src = imgSrc;
          if (modalTitle) modalTitle.textContent = title || "";
          if (modalCategory) modalCategory.textContent = category || "Recent Group";

          lightboxModal.show();
        });
      });
    }

  });
})();
