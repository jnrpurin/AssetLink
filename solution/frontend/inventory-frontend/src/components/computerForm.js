import { createComputer, fetchComputerById, updateComputer } from '../services/api.js';
import { renderComputerList } from './computerList.js';

export async function renderComputerForm(container, computerId = null) {
  let computer = { name: '', brand: '', serialNumber: '', status: 'Available' };

  if (computerId) {
    computer = await fetchComputerById(computerId);
  }

  container.innerHTML = `
    <h2>${computerId ? 'Edit' : 'Add'} Computer</h2>
    <form id="computer-form">
      <input type="text" id="name" placeholder="Name" value="${computer.name}" required />
      <input type="text" id="brand" placeholder="Brand" value="${computer.brand}" required />
      <input type="text" id="serialNumber" placeholder="Serial Number" value="${computer.serialNumber}" required />
      ${computerId ? `<input type="text" id="status" placeholder="Status" value="${computer.status}" required />` : ''}
      <button type="submit">${computerId ? 'Update' : 'Add'}</button>
      <button type="button" id="cancel-btn">Cancel</button>
    </form>
  `;

  const form = document.getElementById('computer-form');
  const cancelBtn = document.getElementById('cancel-btn');

  form.addEventListener('submit', async (e) => {
    e.preventDefault();

    const data = {
      name: document.getElementById('name').value,
      brand: document.getElementById('brand').value,
      serialNumber: document.getElementById('serialNumber').value,
    };

    if (computerId) {
      data.status = document.getElementById('status').value;
      await updateComputer(computerId, data);
    } else {
      await createComputer(data);
    }

    renderComputerList(container);
  });

  cancelBtn.addEventListener('click', () => renderComputerList(container));
}