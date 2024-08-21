import React from 'react';
import { Link } from 'react-router-dom';
import './styles.css';
import logoImage from '../../assets/logo.svg';
import { FiArrowLeft } from 'react-icons/fi';

export default function NewBook() {
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
                <form>
                    <input type="text" placeholder="Title" />
                    <input type="text" placeholder="Author" />
                    <input type="date" />
                    <input type="" placeholder="Price" />
                    <button className="button" type="submit">Add</button>
                </form>
            </div>
        </div>
    );
}