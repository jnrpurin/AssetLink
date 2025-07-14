const API_BASE_URL = 'http://localhost:5072/api';

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

  } catch (error) {
      console.error(`Error deleting computer with ID ${id}:`, error);
      throw error;
  }
}

export async function fetchUsers() { 
  try {
      const response = await fetch(`${API_BASE_URL}/Users`);
      if (!response.ok) {
          const errorData = await response.json();
          throw new Error(errorData.Message || `HTTP error! status: ${response.status}`);
      }
      return await response.json();
  } catch (error) {
      console.error('Error fetching users:', error);
      throw error;
  }
}

export async function assignComputer(assignData) {
  try {
      const response = await fetch(`${API_BASE_URL}/Computers/assign`, {
          method: 'POST',
          headers: {
              'Content-Type': 'application/json',
          },
          body: JSON.stringify(assignData),
      });
      if (!response.ok) {
          const errorData = await response.json();
          throw new Error(errorData.Message || `HTTP error! status: ${response.status}`);
      }
      
      return await response.text();
  } catch (error) {
      console.error('Error assigning computer:', error);
      throw error;
  }
}