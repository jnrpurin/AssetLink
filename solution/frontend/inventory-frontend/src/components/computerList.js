import { fetchComputers, deleteComputer } from '../services/api.js';
import { renderComputerForm } from './computerForm.js';
import { formatUSDate } from '../utils/utils.js';

export async function renderComputerList(container) {
  container.innerHTML = '<p>Loading computers...</p>';

  try {
    const computers = await fetchComputers();

    if (computers.length === 0) {
      container.innerHTML = '<p>No computers registered yet.</p>';
      return;
    }

    const list = document.createElement('div');
    list.className = 'computer-list';

    computers.forEach((computer) => {
      const card = document.createElement('div');
      card.className = 'computer-card';

      // Placeholder image (substitua por sua futura URL ou upload)
      const img = document.createElement('img');
      img.src = computer.imageUrl || 'https://placehold.co/400x200/cccccc/333333?text=No+Image';
      img.alt = computer.name;

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

    container.innerHTML = '';
    container.appendChild(list);

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
            renderComputerList(container);
          } catch (error) {
            alert(`Error deleting computer: ${error.message || 'An unknown error occurred.'}`);
            console.error('Delete error:', error);
          }
        }
      } else if (targetButton.classList.contains('edit-btn')) {
        renderComputerForm(container, id);
      } else if (targetButton.classList.contains('assign-btn')) {
        alert('Assign/Reassign functionality not implemented yet.');
      }
    });
  } catch (error) {
    container.innerHTML = `<p class="text-center text-red-500">Error loading computers: ${error.message}</p>`;
    console.error('Error in renderComputerList:', error);
  }
}
