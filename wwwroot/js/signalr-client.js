"use strict";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/printStatusHub")
    .withAutomaticReconnect()
    .build();

// Listen for any status change and update badge live
connection.on("StatusChanged", function (jobId, newStatus, studentId) {
    const badge = document.getElementById("status-" + jobId);
    if (badge) {
        badge.textContent = newStatus;
        // Reset all classes, apply correct one
        badge.className = "badge " + getStatusBadgeClass(newStatus);
    }

    // Show toast notification
    showToast("Job #" + jobId + " is now: " + newStatus);
});

function getStatusBadgeClass(status) {
    const map = {
        "Queued":    "bg-warning text-dark",
        "Printing":  "bg-primary",
        "Completed": "bg-success",
        "Cancelled": "bg-danger"
    };
    return map[status] || "bg-secondary";
}

function showToast(message) {
    // Bootstrap 5 toast
    const toastEl = document.getElementById("liveToast");
    if (!toastEl) return;
    document.getElementById("toastBody").textContent = message;
    const toast = new bootstrap.Toast(toastEl);
    toast.show();
}

connection.start().catch(err => console.error("SignalR error:", err));
