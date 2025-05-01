// Opens _FeatureAssignModal on Vehicles/create and Vehicles/edit. 

document.addEventListener('DOMContentLoaded', function () {
    const modalEl = document.getElementById('featureModal');
    const modal = new bootstrap.Modal(modalEl);
    const openBtns = document.querySelectorAll('button.btn-assign-features');
    const tableBody = document.getElementById('featureTableBody');

    async function getAllFeatures() {
        const res = await fetch('/Feature/GetAll');
        if (!res.ok) throw new Error(res.statusText);
        return await res.json();
    }

    async function assignFeature(vehicleId, featureId) {
        await fetch(`/Vehicles/AssignFeature?vehicleId=${vehicleId}&featureId=${featureId}`);
    }

    async function removeFeature(vehicleId, featureId) {
        await fetch(`/Vehicles/RemoveFeature?vehicleId=${vehicleId}&featureId=${featureId}`);
    }

    openBtns.forEach(btn => {
        btn.addEventListener('click', async () => {
            const vehicleId = btn.dataset.vehicleId;
            let assigned = JSON.parse(btn.dataset.assigned); // array of feature IDs

            const features = await getAllFeatures();

            // Function to redraw the table
            function refreshTable() {
                tableBody.innerHTML = '';
                features.forEach(f => {
                    const isAssigned = assigned.includes(f.id);
                    const row = document.createElement('tr');

                    if (isAssigned) {
                        // Show "Added" and Remove button
                        row.innerHTML = `
              
              <td>${f.name}</td>
              <td>
                <button class="btn btn-sm btn-secondary" disabled>Added</button>
                <button class="btn btn-sm btn-danger btn-remove-feature"
                        data-feature-id="${f.id}">
                  Remove
                </button>
              </td>`;
                    } else {
                        row.innerHTML = `
              
              <td>${f.name}</td>
              <td>
                <button class="btn btn-sm btn-success btn-add-feature"
                        data-feature-id="${f.id}">
                  Add
                </button>
              </td>`;
                    }

                    tableBody.appendChild(row);
                });

                // Wire up Add buttons
                tableBody.querySelectorAll('.btn-add-feature').forEach(b => {
                    b.addEventListener('click', async () => {
                        const fid = b.dataset.featureId;
                        await assignFeature(vehicleId, fid);
                        assigned.push(Number(fid));
                        refreshTable();
                    });
                });

                // Wire up Remove buttons
                tableBody.querySelectorAll('.btn-remove-feature').forEach(b => {
                    b.addEventListener('click', async () => {
                        const fid = b.dataset.featureId;
                        await removeFeature(vehicleId, fid);
                        assigned = assigned.filter(id => id !== Number(fid));
                        refreshTable();
                    });
                });
            }

            // Initial render and open modal
            refreshTable();
            modal.show();
        });
    });

    // Close
    const closeBtn = document.getElementById('btnCloseFeatures');
    if (closeBtn) {
        closeBtn.addEventListener('click', () => modal.hide());
    }
});
