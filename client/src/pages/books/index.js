import React, { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import api from '../../services/api';
import './styles.css';
import logoImage from '../../assets/logo.svg';
import { FiPower, FiEdit, FiTrash2 } from 'react-icons/fi';

export default function Books() {
    const [books, setBooks] = useState([]);
    const userName = localStorage.getItem('userName');
    const accessToken = localStorage.getItem('accessToken');
    const navigate = useNavigate();

    useEffect(() => {
        api.get('Book/v1/asc/20/1', {
            headers: {
                Authorization: `Bearer ${accessToken}`
            }
        }).then(response => {
            setBooks(response.data.list)
        })
    }, [accessToken]);

    async function logout() {
        try {
            await api.get('Auth/v1/revoke', {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            });

            localStorage.clear();
            navigate('/');
        } catch (error) {
            alert('Logout failed! Try again.');
            console.log(error);
        }
    }

    async function deleteBook(id) {
        try {
            await api.delete(`Book/v1/${id}`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            });

            setBooks(books.filter(book => book.id !== id));
        } catch (error) {
            alert('Delete failed! Try again.');
            console.log(error);
        }
    }

    return (
        <div className="book-container">
            <header>
                <img src={logoImage} alt="Rest With ASP.NET" />
                <span>Welcome, <strong>{userName.toUpperCase()}</strong>!</span>
                <Link className="button" to="/newBook">Add New Book</Link>
                <button type="button" onClick={logout}>
                    <FiPower size={18} color="#251FC5" />
                </button>
            </header>
            <h1>Registered Books</h1>
            <ul>
                {books.map(book => (
                    <li key={book.id}>
                        <strong>Title: </strong>
                        <p>{book.title}</p>
                        <strong>Author: </strong>
                        <p>{book.author}</p>
                        <strong>Price: </strong>
                        <p>{Intl.NumberFormat('pt-br', {style: 'currency', currency: 'BRL'}).format(book.price)}</p>
                        <strong>Release Date: </strong>
                        <p>{Intl.DateTimeFormat('pt-br').format(new Date(book.launchDate))}</p>
                        <button type="button">
                            <FiEdit size={20} color="#251FC5" />
                        </button>
                        <button type="button" onClick={() => deleteBook(book.id)}>
                            <FiTrash2 size={20} color="#251FC5" />
                        </button>
                    </li>
                ))}
            </ul>
        </div>
    );
}