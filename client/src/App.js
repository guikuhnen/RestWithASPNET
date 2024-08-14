import React, { useState } from 'react';
import Header from './Header';

export default function App() {
  const [counter, setCounter] = useState(0);

  function increment() {
    setCounter(counter + 1);
  }

  return (
    <div>
      <Header>
        Counter: {counter}
      </Header>
      <button onClick={increment}>Add</button>
    </div>
  );
}
