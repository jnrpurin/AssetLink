import { createComputer, fetchComputerById, updateComputer } from '../services/api.js';
import { renderComputerList } from './computerList.js';

export async function renderComputerForm(container, computerId = null) {
  let computer = {
    computerManufacturerId: '',
    serialNumber: '',
    specifications: '',
    imageUrl: '',
    purchaseDt: '', 
    warrantyExpirationDt: ''
  };

  if (computerId) {
    const fetchedComputer = await fetchComputerById(computerId);
    if (fetchedComputer) {
      fetchedComputer.purchaseDt = fetchedComputer.purchaseDt ? new Date(fetchedComputer.purchaseDt).toISOString().split('T')[0] : '';
      fetchedComputer.warrantyExpirationDt = fetchedComputer.warrantyExpirationDt ? new Date(fetchedComputer.warrantyExpirationDt).toISOString().split('T')[0] : '';
      computer = fetchedComputer;
    }
  }

  container.innerHTML = `
    <div class="d-flex justify-content-center align-items-start py-4" style="min-height: calc(100vh - 65px);">
      <div class="card shadow-sm w-100" style="max-width: 600px;">
        <div class="card-body p-4">
          <h2 class="card-title text-center mb-4">${computerId ? 'Edit' : 'Add'} Computer</h2>
          <form id="computer-form">
            <div class="mb-3">
              <label for="computerManufacturerId" class="form-label">Manufacturer ID:</label>
              <input type="number" id="computerManufacturerId" placeholder="e.g., 1, 2" value="${computer.computerManufacturerId}" required
                     class="form-control" />
            </div>
            <div class="mb-3">
              <label for="serialNumber" class="form-label">Serial Number:</label>
              <input type="text" id="serialNumber" placeholder="e.g., SN-XYZ-123" value="${computer.serialNumber}" required
                     class="form-control" />
            </div>
            <div class="mb-3">
              <label for="specifications" class="form-label">Specifications:</label>
              <textarea id="specifications" placeholder="e.g., Intel i7, 16GB RAM, 512GB SSD" required
                        class="form-control" rows="4">${computer.specifications}</textarea>
            </div>
            <div class="mb-3">
              <label for="imageUrl" class="form-label">Image URL:</label>
              <input type="url" id="imageUrl" placeholder="e.g., https://example.com/computer.jpg" value="${computer.imageUrl}"
                     class="form-control" />
            </div>
            <div class="mb-3">
              <label for="purchaseDt" class="form-label">Purchase Date:</label>
              <input type="date" id="purchaseDt" value="${computer.purchaseDt}" required
                     class="form-control" />
            </div>
            <div class="mb-4">
              <label for="warrantyExpirationDt" class="form-label">Warranty Expiration Date:</label>
              <input type="date" id="warrantyExpirationDt" value="${computer.warrantyExpirationDt}"
                     class="form-control" />
            </div>
            <div class="d-flex justify-content-end gap-2">
              <button type="submit"
                      class="btn btn-primary">
                ${computerId ? 'Update' : 'Add'}
              </button>
              <button type="button" id="cancel-btn"
                      class="btn btn-secondary">
                Cancel
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  `;

  const form = document.getElementById('computer-form');
  const cancelBtn = document.getElementById('cancel-btn');

  form.addEventListener('submit', async (e) => {
    e.preventDefault();

    const data = {
      computerManufacturerId: parseInt(document.getElementById('computerManufacturerId').value),
      serialNumber: document.getElementById('serialNumber').value,
      specifications: document.getElementById('specifications').value,
      imageUrl: document.getElementById('imageUrl').value,

      purchaseDt: new Date(document.getElementById('purchaseDt').value).toISOString(),
      warrantyExpirationDt: document.getElementById('warrantyExpirationDt').value
        ? new Date(document.getElementById('warrantyExpirationDt').value).toISOString()
        : null 
    };

    try {
      if (computerId) {
        await updateComputer(computerId, data);
        alert('Computer updated successfully!');
      } else {
        await createComputer(data);
        alert('Computer added successfully!');
      }
      renderComputerList(container); 
    } catch (error) {
      alert(`Error: ${error.message || 'An unknown error occurred.'}`);
      console.error('Form submission error:', error);
    }
  });

  cancelBtn.addEventListener('click', () => renderComputerList(container));
}
