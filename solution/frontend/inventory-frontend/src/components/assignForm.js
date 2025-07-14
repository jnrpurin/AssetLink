import { fetchComputerById, fetchUsers, assignComputer } from '../services/api.js';
import { renderComputerList } from './computerList.js';

export async function renderAssignForm(container, computerId) {
  if (!computerId) {
    alert('Computer ID is required to assign a computer.');
    renderComputerList(container);
    return;
  }

  container.innerHTML = '<p class="text-center text-gray-500">Loading assignment form...</p>';

  try {
    const computer = await fetchComputerById(computerId);
    if (!computer) {
      alert('Computer not found.');
      renderComputerList(container);
      return;
    }

    const users = await fetchUsers(); // Fetch list of users
    if (!users || users.length === 0) {
      alert('No users available to assign. Please add users first.');
      renderComputerList(container);
      return;
    }

    const userOptions = users.map(user => `
      <option value="${user.id}">${user.firstName} ${user.lastName} (${user.emailAddress})</option>
    `).join('');

    container.innerHTML = `
      <div class="d-flex justify-content-center align-items-start py-4" style="min-height: calc(100vh - 65px);">
        <div class="card shadow-sm w-100" style="max-width: 600px;">
          <div class="card-body p-4">
            <h2 class="card-title text-center mb-4">Assign Computer: ${computer.serialNumber}</h2>
            <form id="assign-form">
              <div class="mb-3">
                <label for="userId" class="form-label">Assign to User:</label>
                <select id="userId" required class="form-select">
                  <option value="">-- Select a User --</option>
                  ${userOptions}
                </select>
              </div>
              <div class="mb-4">
                <label for="assignStartDt" class="form-label">Assignment Start Date (Optional):</label>
                <input type="date" id="assignStartDt" class="form-control" />
              </div>
              <div class="d-flex justify-content-end gap-2">
                <button type="submit" class="btn btn-primary">
                  Assign
                </button>
                <button type="button" id="cancel-assign-btn" class="btn btn-secondary">
                  Cancel
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    `;

    const form = document.getElementById('assign-form');
    const cancelBtn = document.getElementById('cancel-assign-btn');

    form.addEventListener('submit', async (e) => {
      e.preventDefault();

      const userId = parseInt(document.getElementById('userId').value);
      const assignStartDtInput = document.getElementById('assignStartDt').value;
      
      const assignData = {
        computerId: computer.id,
        userId: userId,
        assignStartDt: assignStartDtInput ? new Date(assignStartDtInput).toISOString() : null
      };

      try {
        await assignComputer(assignData);
        alert('Computer assigned successfully!');
        renderComputerList(container); 
      } catch (error) {
        alert(`Error assigning computer: ${error.message || 'An unknown error occurred.'}`);
        console.error('Assign form submission error:', error);
      }
    });

    cancelBtn.addEventListener('click', () => renderComputerList(container));

  } catch (error) {
    container.innerHTML = `<p class="text-center text-red-500">Error loading assignment form: ${error.message}</p>`;
    console.error('Error in renderAssignForm:', error);
  }
}
