import React from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Login from './pages/login';
import Books from './pages/books';
import NewBook from './pages/newBook';

export default function AppRoutes() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" exact element={<Login />} />
                <Route path="/books" element={<Books />} />
                <Route path="/book/new/:bookId" element={<NewBook />} />
            </Routes>
        </BrowserRouter>
    );
}