const API_URL = 'http://localhost:5072/api/Computers/';

export async function fetchComputers() {
  const response = await fetch(API_URL);
  if (!response.ok) throw new Error('Error to find computers.');
  return await response.json();
}

export async function fetchComputerById(id) {
  const response = await fetch(`${API_URL}/${id}`);
  if (!response.ok) throw new Error('Error to find computer.');
  return await response.json();
}

export async function createComputer(data) {
  const response = await fetch(API_URL, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!response.ok) throw new Error('Error creating new computer.');
  return await response.json();
}

export async function updateComputer(id, data) {
  const response = await fetch(`${API_URL}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!response.ok) throw new Error('Error updating computer.');
}

export async function deleteComputer(id) {
  const response = await fetch(`${API_URL}/${id}`, {
    method: 'DELETE',
  });
  if (!response.ok) throw new Error('Error deliting computer.');
}
