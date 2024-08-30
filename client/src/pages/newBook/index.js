import React, { useState, useEffect } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import api from '../../services/api';
import './styles.css';
import logoImage from '../../assets/logo.svg';
import { FiArrowLeft } from 'react-icons/fi';

export default function NewBook() {
    const [id, setId] = useState(null);
    const [title, setTitle] = useState('');
    const [author, setAuthor] = useState('');
    const [launchDate, setLaunchDate] = useState('');
    const [price, setPrice] = useState('');
    const { bookId } = useParams();
    const navigate = useNavigate();

    const accessToken = localStorage.getItem('accessToken');
    const authorization = {
        headers: {
            Authorization: `Bearer ${accessToken}`
        }
    };

    useEffect(() => {
        if (bookId === '0') {
            return
        } else {
            loadBook();
        }
    }, [bookId]);

    async function loadBook() {
        try {
            const response = await api.get(`Book/v1/${bookId}`, authorization);

            let adjustedDate = response.data.launchDate.split("T", 10)[0];

            setId(response.data.id);
            setTitle(response.data.title);
            setAuthor(response.data.author);
            setLaunchDate(adjustedDate);
            setPrice(response.data.price);
        } catch (error) {
            alert('Error recovering Book! Try again.');
            console.log(error);
            navigate('/books');
        }
    }

    async function saveOrUpdateBook(e) {
        e.preventDefault();

        const data = {
            title,
            author,
            launchDate,
            price
        };

        try {
            if (bookId === '0') {
                await api.post('Book/v1', data, authorization);
            } else {
                data.id = id;
                await api.put('Book/v1', data, authorization);
            }

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
                    <h1>{bookId === '0' ? 'Add' : 'Update'} Book</h1>
                    <p>Enter the book information and click on {bookId === '0' ? `'Add'` : `'Update'`}.</p>
                    <Link className="back-link" to="/books">
                        <FiArrowLeft size={16} color="#251FC5" /> Back to Books
                    </Link>
                </section>
                <form onSubmit={saveOrUpdateBook}>
                    <input type="text" placeholder="Title" value={title} onChange={e => setTitle(e.target.value)} />
                    <input type="text" placeholder="Author" value={author} onChange={e => setAuthor(e.target.value)} />
                    <input type="date" value={launchDate} onChange={e => setLaunchDate(e.target.value)} />
                    <input type="" placeholder="Price" value={price} onChange={e => setPrice(e.target.value)} />
                    <button className="button" type="submit">{bookId === '0' ? 'Add' : 'Update'}</button>
                </form>
            </div>
        </div>
    );
}