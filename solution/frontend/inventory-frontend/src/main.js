import './styles/main.css';
import { renderComputerList } from './components/computerList.js';
import { renderComputerForm } from './components/computerForm.js';

document.addEventListener('DOMContentLoaded', () => {
  const app = document.getElementById('app');

  const header = document.createElement('header');
  header.innerHTML = `
    <h1>Inventory Tracker</h1>
    <button id="add-btn">+ Add Computer</button>
  `;

  const content = document.createElement('main');
  content.id = 'content';

  app.appendChild(header);
  app.appendChild(content);

  renderComputerList(content);

  document.getElementById('add-btn').addEventListener('click', () => {
    renderComputerForm(content);
  });
});
