// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Fallback for environments where SweetAlert CDN is unavailable.
if (!window.Swal || typeof window.Swal.fire !== "function") {
  window.Swal = {
    fire: function (options) {
      const title = options?.title || "Confirm action";
      const text = options?.text ? "\n\n" + options.text : "";
      const confirmed = window.confirm(title + text);
      return Promise.resolve({ isConfirmed: confirmed });
    }
  };
}

document.addEventListener("click", (event) => {
  const target = event.target;
  if (!(target instanceof HTMLElement)) return;

  const row = target.closest("tr[data-href]");
  if (row && !target.closest(".icon-action") && !target.closest("a") && !target.closest("button")) {
    const href = row.getAttribute("data-href");
    if (href) window.location.href = href;
  }

  const deleteLink = target.closest("[data-confirm]");
  if (deleteLink && deleteLink.getAttribute("data-confirm") === "delete") {
    event.preventDefault();
    const href = deleteLink.getAttribute("href");
    Swal.fire({
      title: "Archive this record?",
      text: "This action is a soft delete. You can restore it later.",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Archive",
      cancelButtonText: "Cancel",
      confirmButtonColor: "#c2463a"
    }).then((result) => {
      if (result.isConfirmed && href) {
        window.location.href = href;
      }
    });
  }

  const deleteForm = target.closest("form[data-confirm]");
  if (deleteForm && deleteForm.getAttribute("data-confirm") === "delete") {
    if (target.closest("a") || target.closest("[type='button']")) {
      return;
    }
    event.preventDefault();
    Swal.fire({
      title: "Archive this record?",
      text: "This action is a soft delete. You can restore it later.",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Archive",
      cancelButtonText: "Cancel",
      confirmButtonColor: "#c2463a"
    }).then((result) => {
      if (result.isConfirmed) {
        deleteForm.submit();
      }
    });
  }

  const softDeleteForm = target.closest("form[data-confirm]");
  if (softDeleteForm && softDeleteForm.getAttribute("data-confirm") === "soft-delete") {
    if (target.closest("a") || target.closest("[type='button']")) {
      return;
    }
    event.preventDefault();
    Swal.fire({
      title: "Retire this record?",
      text: "Soft delete. You can restore it within 1 year.",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Retire",
      cancelButtonText: "Cancel",
      confirmButtonColor: "#c2463a"
    }).then((result) => {
      if (result.isConfirmed) {
        softDeleteForm.submit();
      }
    });
  }

  const approveForm = target.closest("form[data-confirm='report-approve']");
  if (approveForm) {
    if (target.closest("a") || target.closest("[type='button']")) {
      return;
    }
    event.preventDefault();
    Swal.fire({
      title: "Approve this report?",
      text: "This marks the report as internally approved.",
      icon: "question",
      showCancelButton: true,
      confirmButtonText: "Approve",
      cancelButtonText: "Cancel",
      confirmButtonColor: "#2f6b4f"
    }).then((result) => {
      if (result.isConfirmed) {
        approveForm.submit();
      }
    });
  }

  const rejectForm = target.closest("form[data-confirm='report-reject']");
  if (rejectForm) {
    if (target.closest("a") || target.closest("[type='button']")) {
      return;
    }
    event.preventDefault();
    Swal.fire({
      title: "Reject this report?",
      text: "This marks the report as rejected and requires correction.",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Reject",
      cancelButtonText: "Cancel",
      confirmButtonColor: "#b63d33"
    }).then((result) => {
      if (result.isConfirmed) {
        rejectForm.submit();
      }
    });
  }

  const orderApproveForm = target.closest("form[data-confirm='order-approve']");
  if (orderApproveForm) {
    if (target.closest("a") || target.closest("[type='button']")) {
      return;
    }
    event.preventDefault();
    Swal.fire({
      title: "Approve this order?",
      text: "This will finalize internal approval for the order.",
      icon: "question",
      showCancelButton: true,
      confirmButtonText: "Approve",
      cancelButtonText: "Cancel",
      confirmButtonColor: "#2f6b4f"
    }).then((result) => {
      if (result.isConfirmed) {
        orderApproveForm.submit();
      }
    });
  }

  const orderRejectForm = target.closest("form[data-confirm='order-reject']");
  if (orderRejectForm) {
    if (target.closest("a") || target.closest("[type='button']")) {
      return;
    }
    event.preventDefault();
    Swal.fire({
      title: "Reject this order?",
      text: "This will mark the order as rejected/cancelled.",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Reject",
      cancelButtonText: "Cancel",
      confirmButtonColor: "#b63d33"
    }).then((result) => {
      if (result.isConfirmed) {
        orderRejectForm.submit();
      }
    });
  }

  const supplierApproveForm = target.closest("form[data-confirm='supplier-approve']");
  if (supplierApproveForm) {
    if (target.closest("a") || target.closest("[type='button']")) {
      return;
    }
    event.preventDefault();
    Swal.fire({
      title: "Approve this supplier?",
      text: "This will mark supplier compliance as approved.",
      icon: "question",
      showCancelButton: true,
      confirmButtonText: "Approve",
      cancelButtonText: "Cancel",
      confirmButtonColor: "#2f6b4f"
    }).then((result) => {
      if (result.isConfirmed) {
        supplierApproveForm.submit();
      }
    });
  }

  const supplierRejectForm = target.closest("form[data-confirm='supplier-reject']");
  if (supplierRejectForm) {
    if (target.closest("a") || target.closest("[type='button']")) {
      return;
    }
    event.preventDefault();
    Swal.fire({
      title: "Reject this supplier?",
      text: "This will mark supplier compliance as rejected.",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Reject",
      cancelButtonText: "Cancel",
      confirmButtonColor: "#b63d33"
    }).then((result) => {
      if (result.isConfirmed) {
        supplierRejectForm.submit();
      }
    });
  }

  const policyApproveForm = target.closest("form[data-confirm='policy-approve']");
  if (policyApproveForm) {
    if (target.closest("a") || target.closest("[type='button']")) {
      return;
    }
    event.preventDefault();
    Swal.fire({
      title: "Approve this policy?",
      text: "This will publish the policy as active.",
      icon: "question",
      showCancelButton: true,
      confirmButtonText: "Approve",
      cancelButtonText: "Cancel",
      confirmButtonColor: "#2f6b4f"
    }).then((result) => {
      if (result.isConfirmed) {
        policyApproveForm.submit();
      }
    });
  }

  const policyRejectForm = target.closest("form[data-confirm='policy-reject']");
  if (policyRejectForm) {
    if (target.closest("a") || target.closest("[type='button']")) {
      return;
    }
    event.preventDefault();
    Swal.fire({
      title: "Reject this policy?",
      text: "This will keep the policy in draft for revision.",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Reject",
      cancelButtonText: "Cancel",
      confirmButtonColor: "#b63d33"
    }).then((result) => {
      if (result.isConfirmed) {
        policyRejectForm.submit();
      }
    });
  }

  const restoreForm = target.closest("form[data-confirm='restore']");
  if (restoreForm) {
    if (target.closest("a") || target.closest("[type='button']")) {
      return;
    }
    event.preventDefault();
    Swal.fire({
      title: "Restore this record?",
      text: "This will un-archive the record and make it active again.",
      icon: "question",
      showCancelButton: true,
      confirmButtonText: "Restore",
      cancelButtonText: "Cancel",
      confirmButtonColor: "#2f6b4f"
    }).then((result) => {
      if (result.isConfirmed) {
        restoreForm.submit();
      }
    });
  }

  const supplierDocReviewForm = target.closest("form[data-confirm='supplier-doc-review']");
  if (supplierDocReviewForm) {
    if (target.closest("a") || target.closest("[type='button']")) {
      return;
    }
    event.preventDefault();
    Swal.fire({
      title: "Mark document as reviewed?",
      text: "This confirms compliance review for this supplier document.",
      icon: "question",
      showCancelButton: true,
      confirmButtonText: "Review",
      cancelButtonText: "Cancel",
      confirmButtonColor: "#2f6b4f"
    }).then((result) => {
      if (result.isConfirmed) {
        supplierDocReviewForm.submit();
      }
    });
  }

  const supplierDocFlagForm = target.closest("form[data-confirm='supplier-doc-flag']");
  if (supplierDocFlagForm) {
    if (target.closest("a") || target.closest("[type='button']")) {
      return;
    }
    event.preventDefault();
    Swal.fire({
      title: "Flag this document?",
      text: "This will require follow-up before compliance can proceed.",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Flag",
      cancelButtonText: "Cancel",
      confirmButtonColor: "#b63d33"
    }).then((result) => {
      if (result.isConfirmed) {
        supplierDocFlagForm.submit();
      }
    });
  }

  const rejectRequestForm = target.closest("form[data-confirm='reject-request']");
  if (rejectRequestForm) {
    if (target.closest("a") || target.closest("[type='button']")) {
      return;
    }
    event.preventDefault();
    Swal.fire({
      title: "Reject this request?",
      text: "This will decline the override request and notify the requester.",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Reject",
      cancelButtonText: "Cancel",
      confirmButtonColor: "#b63d33"
    }).then((result) => {
      if (result.isConfirmed) {
        rejectRequestForm.submit();
      }
    });
  }
});

document.addEventListener("DOMContentLoaded", () => {
  // System-wide table alignment normalizer:
  // (Disabled temporarily to fix disappearing actions)
  /*
  const allTables = document.querySelectorAll("table.table");
  ...
  */
  if (false) { // Skip this block for now
    const allTables = document.querySelectorAll("table.table");
    const centerKeywords = /\b(status|approval|risk|level|state)\b/i;
    const rightKeywords = /\b(amount|total|cost|price|qty|quantity|rating|score|percent|%|expiry|date|submitted|created|updated|value)\b/i;
    const actionsKeywords = /\bactions?\b/i;

    for (const table of allTables) {
      table.classList.add("erp-auto-table-lock");
      const headerRow = table.querySelector("thead tr");
      const bodyRows = table.querySelectorAll("tbody tr");
      if (!headerRow || bodyRows.length === 0) continue;

      const headers = Array.from(headerRow.children).filter((cell) => cell.tagName === "TH");
      headers.forEach((th, idx) => {
        const label = (th.textContent || "").trim();
        let alignClass = "";

        if (th.classList.contains("table-cell-right")) {
          alignClass = "table-cell-right";
        } else if (th.classList.contains("table-cell-center")) {
          alignClass = "table-cell-center";
        } else if (actionsKeywords.test(label)) {
          // Skip automatic alignment for actions to avoid layout breaking
          alignClass = "";
        } else if (centerKeywords.test(label)) {
          alignClass = "table-cell-center";
        } else if (rightKeywords.test(label)) {
          alignClass = "table-cell-right";
        }

        if (alignClass) {
          th.classList.add(alignClass);
          for (const row of bodyRows) {
            const cell = row.children[idx];
            if (cell) {
              cell.classList.add(alignClass);
            }
          }
        }

        if (actionsKeywords.test(label)) {
          th.classList.add("table-actions-col");
          th.classList.add("actions-cell");
          for (const row of bodyRows) {
            const cell = row.children[idx];
            if (cell) {
              cell.classList.add("table-actions-col");
              cell.classList.add("actions-cell");
            }
          }
        }
      });

      const bodyCells = table.querySelectorAll("tbody td");
      for (const cell of bodyCells) {
        if (cell.querySelector(".icon-action")) {
          // Skip redundant class assignment to avoid DataTables conflicts
        }
      }
    }
  }

  if (window.bootstrap && typeof window.bootstrap.Tooltip === "function") {
    const tooltipTriggers = document.querySelectorAll("[data-bs-toggle='tooltip']");
    for (const trigger of tooltipTriggers) {
      new window.bootstrap.Tooltip(trigger);
    }
  }

  const mobileToggle = document.querySelector(".mobile-nav-toggle");
  const sidebarOverlay = document.querySelector(".sidebar-overlay");
  const sidebarLinks = document.querySelectorAll(".erp-sidebar .sidebar-nav a");
  const mobileViewport = window.matchMedia("(max-width: 1100px)");

  // Highlight current sidebar location with a pressed/active state.
  const normalizedCurrentPath = (window.location.pathname || "/")
    .toLowerCase()
    .replace(/\/+$/, "") || "/";
  let activeLink = null;
  let activeLength = -1;
  for (const link of sidebarLinks) {
    if (!(link instanceof HTMLAnchorElement)) continue;
    const path = (new URL(link.href, window.location.origin).pathname || "/")
      .toLowerCase()
      .replace(/\/+$/, "") || "/";
    if (normalizedCurrentPath === path || (path !== "/" && normalizedCurrentPath.startsWith(path))) {
      if (path.length > activeLength) {
        activeLength = path.length;
        activeLink = link;
      }
    }
  }
  if (activeLink) {
    activeLink.classList.add("active");
  }

  if (mobileViewport.matches) {
    document.body.classList.remove("sidebar-open");
  }

  if (mobileToggle && sidebarOverlay) {
    mobileToggle.addEventListener("click", () => {
      document.body.classList.toggle("sidebar-open");
    });

    sidebarOverlay.addEventListener("click", () => {
      document.body.classList.remove("sidebar-open");
    });

    for (const link of sidebarLinks) {
      link.addEventListener("click", () => {
        document.body.classList.remove("sidebar-open");
      });
    }
  }

  mobileViewport.addEventListener("change", (event) => {
    if (!event.matches) {
      document.body.classList.remove("sidebar-open");
    }
  });

  const rateEl = document.getElementById("topbar-rate");
  const tempEl = document.getElementById("topbar-temp");
  const cityEl = document.getElementById("topbar-city");
  const profileMenu = document.querySelector(".profile-menu");
  const profileTrigger = document.querySelector(".profile-trigger");
  const quickLogoutIcon = document.querySelector(".logout-indicator[data-logout-now='true']");
  const quickLogoutForm = document.getElementById("quickLogoutForm");

  if (profileMenu instanceof HTMLElement && profileTrigger instanceof HTMLElement) {
    profileTrigger.addEventListener("click", (event) => {
      event.preventDefault();
      profileMenu.classList.toggle("open");
    });

    document.addEventListener("click", (event) => {
      if (!profileMenu.contains(event.target)) {
        profileMenu.classList.remove("open");
      }
    });
  }

  if (quickLogoutIcon instanceof HTMLElement && quickLogoutForm instanceof HTMLFormElement) {
    quickLogoutIcon.addEventListener("click", (event) => {
      event.preventDefault();
      event.stopPropagation();
      quickLogoutForm.submit();
    });
  }

  const notices = document.querySelectorAll(".notice");
  for (const notice of notices) {
    const close = notice.querySelector(".notice-close");
    if (close) {
      close.addEventListener("click", () => {
        notice.remove();
      });
    }

    window.setTimeout(() => {
      if (notice.isConnected) {
        notice.remove();
      }
    }, 5000);
  }

  const semanticStatusClasses = [
    "status-enterprise--approved",
    "status-enterprise--pending",
    "status-enterprise--draft",
    "status-enterprise--warning",
    "status-enterprise--risk"
  ];

  const classifyStatusText = (rawText) => {
    const text = (rawText || "").toUpperCase().trim();
    if (!text) return null;

    if (
      text.includes("BLOCKED") ||
      text.includes("REJECTED") ||
      text.includes("HIGH RISK") ||
      text.includes("RISK") ||
      text.includes("NON-COMPLIANT") ||
      text.includes("ELIGIBLE: NO")
    ) {
      return "status-enterprise--risk";
    }

    if (
      text.includes("SUSPENDED") ||
      text.includes("ON HOLD") ||
      text.includes("WARNING")
    ) {
      return "status-enterprise--warning";
    }

    if (text.includes("DRAFT")) {
      return "status-enterprise--draft";
    }

    if (
      text.includes("SUBMITTED") ||
      text.includes("PENDING") ||
      text.includes("IN REVIEW") ||
      text.includes("NEW")
    ) {
      return "status-enterprise--pending";
    }

    if (
      text.includes("ACTIVE") ||
      text.includes("APPROVED") ||
      text.includes("ACCEPTED") ||
      text.includes("COMPLIANT") ||
      text.includes("ELIGIBLE: YES") ||
      text.includes("CLEAR")
    ) {
      return "status-enterprise--approved";
    }

    return null;
  };

  const statusNodes = document.querySelectorAll(".badge, .status-chip, .approval-pill");
  for (const node of statusNodes) {
    node.classList.remove(...semanticStatusClasses);
    const semanticClass = classifyStatusText(node.textContent);
    if (semanticClass) {
      node.classList.add(semanticClass);
    }
  }

  const actionBars = document.querySelectorAll(
    ".hero-actions, .page-header-actions, .module-header-actions, .header-action-group, .empty-state-actions, .filter-actions, .doc-upload-actions, .procurement-approval-actions, .report-override-controls"
  );
  for (const bar of actionBars) {
    bar.classList.add("ui-action-bar");
  }

  const actionControls = document.querySelectorAll(".ui-action-bar .btn, .ui-action-bar .button");
  for (const control of actionControls) {
    const text = (control.textContent || "").toLowerCase();
    const hasClass = (name) => control.classList.contains(name);

    if (
      hasClass("btn-danger") ||
      text.includes("delete") ||
      text.includes("archive") ||
      text.includes("reject") ||
      text.includes("retire") ||
      text.includes("flag")
    ) {
      control.classList.add("ui-btn-danger");
      continue;
    }

    if (
      hasClass("btn-success") ||
      hasClass("btn-primary") ||
      text.includes("save") ||
      text.includes("submit") ||
      text.includes("approve") ||
      text.includes("create") ||
      text.includes("add ")
    ) {
      control.classList.add("ui-btn-primary");
      continue;
    }

    if (
      hasClass("btn-outline-secondary") ||
      hasClass("btn-outline-primary") ||
      text.includes("edit") ||
      text.includes("cancel") ||
      text.includes("back")
    ) {
      control.classList.add("ui-btn-secondary");
    }
  }

  if (!rateEl && !tempEl) return;

  function loadWidgets(attempt) {
    fetch("/Dashboard/Widgets?baseCurrency=PHP", { credentials: "same-origin" })
      .then(function (response) {
        if (!response.ok) throw new Error(response.status);
        return response.json();
      })
      .then(function (data) {
        if (!data) return;

        if (data.exchangeRates && Array.isArray(data.exchangeRates.rates)) {
          var usdRate = data.exchangeRates.rates.find(function (r) { return r.currency === "USD"; });
          if (usdRate && rateEl) {
            var formatter = new Intl.NumberFormat("en-PH", { style: "currency", currency: "PHP" });
            rateEl.textContent = "1 USD = " + formatter.format(1 / usdRate.rate);
          }
        }

        if (data.weather && tempEl) {
          tempEl.textContent = data.weather.temperatureC + "\u00B0C";
          if (cityEl && data.weather.city) {
            cityEl.textContent = data.weather.city.replace(" City", "");
          }
        }
      })
      .catch(function () {
        if (attempt < 3) {
          setTimeout(function () { loadWidgets(attempt + 1); }, 2000 * attempt);
        }
      });
  }

  loadWidgets(1);
});