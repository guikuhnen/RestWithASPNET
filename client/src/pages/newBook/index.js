import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import api from '../../services/api';
import './styles.css';
import logoImage from '../../assets/logo.svg';
import { FiArrowLeft } from 'react-icons/fi';

export default function NewBook() {
    const [title, setTitle] = useState('');
    const [author, setAuthor] = useState('');
    const [launchDate, setLaunchDate] = useState('');
    const [price, setPrice] = useState('');
    const navigate = useNavigate();

    async function createNewBook(e) {
        e.preventDefault();

        const data = {
            title,
            author,
            launchDate,
            price
        };

        const accessToken = localStorage.getItem('accessToken');

        try {
            await api.post('Book/v1', data, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            });

            navigate('/books');
        } catch (error) {
            alert('Error while recording Book! Try again.');
            console.log(error);
        }
    }

    return (
        <div className="new-book-container">
            <div className="content">
                <section className="form">
                    <img src={logoImage} alt="Rest With ASP.NET" />
                    <h1>Add New Book</h1>
                    <p>Enter the book information and click on 'Add'!</p>
                    <Link className="back-link" to="/books">
                        <FiArrowLeft size={16} color="#251FC5" /> Home
                    </Link>
                </section>
                <form onSubmit={createNewBook}>
                    <input type="text" placeholder="Title" value={title} onChange={e => setTitle(e.target.value)} />
                    <input type="text" placeholder="Author" value={author} onChange={e => setAuthor(e.target.value)} />
                    <input type="date" value={launchDate} onChange={e => setLaunchDate(e.target.value)} />
                    <input type="" placeholder="Price" value={price} onChange={e => setPrice(e.target.value)} />
                    <button className="button" type="submit">Add</button>
                </form>
            </div>
        </div>
    );
}