import { fetchComputers, deleteComputer } from '../services/api.js';
import { renderComputerForm } from './computerForm.js';
import { renderAssignForm } from './assignForm.js';
import { formatUSDate } from '../utils/utils.js';

let currentPage = 1;
const itemsPerPage = 5; 
let currentFilters = {
  serialNumber: '',
  assignedTo: '',
  status: ''
};

export async function renderComputerList(container) {
  container.innerHTML = '<p class="text-center text-muted">Loading computers...</p>';

  try {
    const allComputers = await fetchComputers(); 

    if (allComputers.length === 0) {
      container.innerHTML = `
        <div class="text-center py-4">
          <p class="text-muted">No computers registered yet.</p>
          <button id="add-first-computer-btn"
                  class="btn btn-primary mt-3">
            + Add First Computer
          </button>
        </div>
      `;
      document.getElementById('add-first-computer-btn').addEventListener('click', () => renderComputerForm(container));
      return;
    }

    // --- Filters ---
    const filterContainer = document.createElement('div');
    filterContainer.className = 'container py-3';
    filterContainer.innerHTML = `
      <div class="card shadow-sm mb-4">
        <div class="card-body">
          <h5 class="card-title mb-3">Filter Computers</h5>
          <div class="row g-3">
            <div class="col-md-4">
              <label for="filterSerialNumber" class="form-label">Serial Number:</label>
              <input type="text" class="form-control" id="filterSerialNumber" placeholder="Enter serial number" value="${currentFilters.serialNumber}">
            </div>
            <div class="col-md-4">
              <label for="filterAssignedTo" class="form-label">Assigned To:</label>
              <input type="text" class="form-control" id="filterAssignedTo" placeholder="Enter user name" value="${currentFilters.assignedTo}">
            </div>
            <div class="col-md-4">
              <label for="filterStatus" class="form-label">Status:</label>
              <select class="form-select" id="filterStatus">
                <option value="">All Statuses</option>
                <option value="in_use" ${currentFilters.status === 'in_use' ? 'selected' : ''}>In Use</option>
                <option value="available" ${currentFilters.status === 'available' ? 'selected' : ''}>Available</option>
                <option value="in_maintenance" ${currentFilters.status === 'in_maintenance' ? 'selected' : ''}>In Maintenance</option>
                <option value="retired" ${currentFilters.status === 'retired' ? 'selected' : ''}>Retired</option>
                <!-- Adicione mais opções de status conforme necessário -->
              </select>
            </div>
          </div>
          <div class="d-flex justify-content-end mt-3">
            <button id="apply-filter-btn" class="btn btn-primary me-2">Apply Filters</button>
            <button id="clear-filter-btn" class="btn btn-secondary">Clear Filters</button>
          </div>
        </div>
      </div>
    `;
    container.innerHTML = ''; 
    container.appendChild(filterContainer);
    
    let filteredComputers = allComputers.filter(computer => {
      const matchesSerialNumber = currentFilters.serialNumber ?
        computer.serialNumber.toLowerCase().includes(currentFilters.serialNumber.toLowerCase()) : true;

      const assignedToFullName = (computer.assignedToUserFirstName && computer.assignedToUserLastName)
        ? `${computer.assignedToUserFirstName} ${computer.assignedToUserLastName}`
        : '';
      const matchesAssignedTo = currentFilters.assignedTo ?
        assignedToFullName.toLowerCase().includes(currentFilters.assignedTo.toLowerCase()) : true;

      const matchesStatus = currentFilters.status ?
        (computer.status && computer.status.toLowerCase() === currentFilters.status.toLowerCase()) : true;

      return matchesSerialNumber && matchesAssignedTo && matchesStatus;
    });
    // --- end filters ---


    const totalPages = Math.ceil(filteredComputers.length / itemsPerPage);

    if (currentPage > totalPages && totalPages > 0) {
        currentPage = totalPages;
    } else if (totalPages === 0) {
        currentPage = 1;
    }

    const startIndex = (currentPage - 1) * itemsPerPage;
    const endIndex = startIndex + itemsPerPage;
    const computersToDisplay = filteredComputers.slice(startIndex, endIndex);

    const list = document.createElement('div');
    list.className = 'computer-list'; 

    if (computersToDisplay.length === 0 && filteredComputers.length > 0) {
        list.innerHTML = `<p class="text-center text-muted">No computers found on this page with current filters. Try adjusting the page number.</p>`;
    } else if (computersToDisplay.length === 0 && filteredComputers.length === 0) {
        list.innerHTML = `<p class="text-center text-muted">No computers match your filter criteria.</p>`;
    }

    computersToDisplay.forEach((computer) => {
      const card = document.createElement('div');
      card.className = 'computer-card'; 

      const img = document.createElement('img');
      img.src = computer.imageUrl || 'https://placehold.co/400x200/cccccc/333333?text=No+Image';
      img.alt = computer.serialNumber;

      const info = document.createElement('div');
      info.className = 'computer-info'; 

      const col1 = document.createElement('div');
      col1.className = 'computer-column'; 
      col1.innerHTML = `
        <div><strong>Serial Number:</strong> ${computer.serialNumber || '-'}</div>
        <div><strong>Manufacturer:</strong> ${computer.computerManufacturerName || '-'}</div>
        <div><strong>Status:</strong> ${computer.status || '-'}</div>
        <div><strong>Specifications:</strong></div>
        <div class="specifications-text">${computer.specifications || '-'}</div>
      `;

      const col2 = document.createElement('div');
      col2.className = 'computer-column'; 
      col2.innerHTML = `
        <div><strong>Purchased on:</strong> ${formatUSDate(computer.purchaseDt) || '-'} </div>
        <div><strong>Warranty until:</strong> ${formatUSDate(computer.warrantyExpirationDt) || '-'} </div>
        <div><strong>Assigned on:</strong> ${formatUSDate(computer.assignedOnDt) || '-'} </div>
        <div><strong>Assigned to:</strong> ${computer.assignedTo || '-'} </div>
      `;

      info.appendChild(col1);
      info.appendChild(col2);

      const actions = document.createElement('div');
      actions.className = 'computer-actions'; 
      actions.innerHTML = `
        <button class="assign-btn" data-id="${computer.id}" title="Assign or Reassign">
          <i class="bi bi-person-check"></i>
        </button>
        <button class="edit-btn" data-id="${computer.id}" title="Edit">
          <i class="bi bi-pencil-square"></i>
        </button>
        <button class="delete-btn" data-id="${computer.id}" title="Delete">
          <i class="bi bi-trash"></i>
        </button>
      `;

      card.appendChild(img);
      card.appendChild(info);
      card.appendChild(actions);
      list.appendChild(card);
    });

    container.appendChild(list);

    const paginationNav = document.createElement('nav');
    paginationNav.setAttribute('aria-label', 'Page navigation');
    paginationNav.className = 'mt-4 d-flex justify-content-center';
    paginationNav.innerHTML = `
      <ul class="pagination">
        <li class="page-item ${currentPage === 1 ? 'disabled' : ''}">
          <a class="page-link" href="#" data-page="${currentPage - 1}">Previous</a>
        </li>
        ${Array.from({ length: totalPages }, (_, i) => i + 1).map(page => `
          <li class="page-item ${currentPage === page ? 'active' : ''}">
            <a class="page-link" href="#" data-page="${page}">${page}</a>
          </li>
        `).join('')}
        <li class="page-item ${currentPage === totalPages ? 'disabled' : ''}">
          <a class="page-link" href="#" data-page="${currentPage + 1}">Next</a>
        </li>
      </ul>
    `;

    if (totalPages > 1) {
        container.appendChild(paginationNav);
    }

    paginationNav.addEventListener('click', (e) => {
      e.preventDefault();
      const pageLink = e.target.closest('.page-link');
      if (pageLink) {
        const newPage = parseInt(pageLink.dataset.page);
        if (newPage > 0 && newPage <= totalPages && newPage !== currentPage) {
          currentPage = newPage;
          renderComputerList(container);
        }
      }
    });

    document.getElementById('apply-filter-btn').addEventListener('click', () => {
      currentFilters.serialNumber = document.getElementById('filterSerialNumber').value;
      currentFilters.assignedTo = document.getElementById('filterAssignedTo').value;
      currentFilters.status = document.getElementById('filterStatus').value;
      currentPage = 1;
      renderComputerList(container);
    });

    document.getElementById('clear-filter-btn').addEventListener('click', () => {
      currentFilters = { serialNumber: '', assignedTo: '', status: '' };
      currentPage = 1;
      renderComputerList(container);
    });


    // Event listeners for Delete, Edit, Assign
    list.addEventListener('click', async (e) => {
      const targetButton = e.target.closest('button');
      if (!targetButton) return;

      const id = targetButton.dataset.id;
      if (!id) return;

      if (targetButton.classList.contains('delete-btn')) {
        if (confirm('Do you really want to delete this computer?')) {
          try {
            await deleteComputer(id);
            alert('Computer deleted successfully!');
            if (computersToDisplay.length === 1 && currentPage > 1 && filteredComputers.length -1 <= startIndex) {
                currentPage--;
            }
            renderComputerList(container);
          } catch (error) {
            alert(`Error deleting computer: ${error.message || 'An unknown error occurred.'}`);
            console.error('Delete error:', error);
          }
        }
      } else if (targetButton.classList.contains('edit-btn')) {
        renderComputerForm(container, id);
      } else if (targetButton.classList.contains('assign-btn')) {
        renderAssignForm(container, id);
      }
    });
  } catch (error) {
    container.innerHTML = `<p class="text-center text-danger">Error loading computers: ${error.message}</p>`;
    console.error('Error in renderComputerList:', error);
  }
}
