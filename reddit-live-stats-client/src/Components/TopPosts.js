import React, { useEffect, useState } from 'react';
import axios from 'axios';

const TopPosts = () => {
  const [posts, setPosts] = useState([]);

  useEffect(() => {
    const fetchData = () => {
      axios.get('https://localhost:7295/Stats/top-posts?count=10')
        .then(response => setPosts(response.data))
        .catch(error => console.error('Error fetching top posts:', error));
    };

    fetchData(); // Initial fetch
    const intervalId = setInterval(fetchData, 5000); // Fetch data every 5 seconds

    return () => clearInterval(intervalId);
  }, []);

  return (
    <div>
      <h2>Top Posts</h2>
      <ul>
        {posts.map(post => (
          <li key={post.id}>{post.title} by {post.author} - Upvotes: {post.upVotes}</li>
        ))}
      </ul>
    </div>
  );
};

export default TopPosts;
