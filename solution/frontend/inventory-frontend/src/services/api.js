const API_URL = 'http://localhost:5072/api/Computers/';
const API_BASE_URL = 'http://localhost:5072/api';

export async function fetchComputers_old() {
  const response = await fetch(API_URL);
  if (!response.ok) throw new Error('Error to find computers.');
  return await response.json();
}

export async function fetchComputers() {
  try {
      const response = await fetch(`${API_BASE_URL}/Computers`);
      if (!response.ok) {
          const errorData = await response.json();
          throw new Error(errorData.Message || `HTTP error! status: ${response.status}`);
      }
      return await response.json();
  } catch (error) {
      console.error('Error fetching computers:', error);
      throw error;
  }
}

export async function fetchComputerById(id) {
  try {
      const response = await fetch(`${API_BASE_URL}/Computers/${id}`);
      if (!response.ok) {
          const errorData = await response.json();
          throw new Error(errorData.Message || `HTTP error! status: ${response.status}`);
      }
      return await response.json();
  } catch (error) {
      console.error(`Error fetching computer with ID ${id}:`, error);
      throw error;
  }
}

export async function createComputer_old(data) {
  const response = await fetch(API_URL, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!response.ok) throw new Error('Error creating new computer.');
  return await response.json();
}

export async function createComputer(computerData) {
  try {
      const response = await fetch(`${API_BASE_URL}/Computers`, {
          method: 'POST',
          headers: {
              'Content-Type': 'application/json',
          },
          body: JSON.stringify(computerData),
      });
      if (!response.ok) {
          const errorData = await response.json();
          throw new Error(errorData.Message || `HTTP error! status: ${response.status}`);
      }
      return await response.json(); 
  } catch (error) {
      console.error('Error creating computer:', error);
      throw error;
  }
}

export async function updateComputer(id, computerData) {
  try {
      const response = await fetch(`${API_BASE_URL}/Computers/${id}`, {
          method: 'PUT',
          headers: {
              'Content-Type': 'application/json',
          },
          body: JSON.stringify(computerData),
      });
      if (!response.ok) {
          const errorData = await response.json();
          throw new Error(errorData.Message || `HTTP error! status: ${response.status}`);
      }
  } catch (error) {
      console.error(`Error updating computer with ID ${id}:`, error);
      throw error;
  }
}

export async function deleteComputer(id) {
  try {
      const response = await fetch(`${API_BASE_URL}/Computers/${id}`, {
          method: 'DELETE',
      });
      if (!response.ok) {
          const errorData = await response.json();
          throw new Error(errorData.Message || `HTTP error! status: ${response.status}`);
      }
      // No content to return for 204 No Content
  } catch (error) {
      console.error(`Error deleting computer with ID ${id}:`, error);
      throw error;
  }
}

// Placeholder for assignComputer (to be implemented later)
export async function assignComputer(assignData) {
  // This function will be implemented when we work on the assign feature
  console.log('Assign computer data:', assignData);
  throw new Error('Assign computer functionality not implemented yet.');
}