import React, { useEffect, useState } from 'react';
import axios from 'axios';

const TopUsers = () => {
  const [users, setUsers] = useState([]);

  useEffect(() => {
    const fetchData = () => {
      axios.get('https://localhost:7295/Stats/top-users?count=10000')
          .then(response => setUsers(response.data.slice(0, 10))) // Slice the array to get only the top 10 posts
          .catch(error => console.error('Error fetching top posts:', error));
    };

    fetchData(); // Initial fetch
    const intervalId = setInterval(fetchData, 5000); // Fetch data every 5 seconds

    return () => clearInterval(intervalId);
  }, []);

  return (
    <div>
      <h2>Top Users</h2>
      <ul>
        {users.map(user => (
          <li key={user.userName}>{user.userName} - Posts: {user.postCount}, Upvotes: {user.upVotes}</li>
        ))}
      </ul>
    </div>
  );
};

export default TopUsers;
