import React from 'react';
import Header from './Components/Header';
import TopPosts from './Components/TopPosts';
import TopUsers from './Components/TopUsers';
import './App.css';

const App = () => {
    return (
        <div className="App">
            <Header />
            <TopPosts />
            <TopUsers />
        </div>
    );
};

export default App;
