import './App.css';
import 'react-toastify/dist/ReactToastify.css';
import {
  BrowserRouter,
  Routes,
  Route
} from 'react-router-dom';
import { ToastContainer, toast } from 'react-toastify';

import Home from './pages/Home';
import Login from './pages/Login';
import Register from './pages/Register';

import Navbar from './components/Navbar';
import Tool from './pages/tool/Tool';
import Password from './pages/password/Password';
import Note from './pages/note/Note';
import Profile from './pages/profile/Profile';

function App() {
  return (
    <div className="App">
      <BrowserRouter>
        <Navbar />
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/tool" element={<Tool />} />
          <Route path="/password" element={<Password />} />
          <Route path="/note" element={<Note />} />
          <Route path="/profile" element={<Profile />} />
        </Routes>
        <ToastContainer
          draggableDirection='y'
          draggable={true}
          autoClose={2500}
          position={toast.POSITION.BOTTOM_CENTER}
        />
      </BrowserRouter>
    </div>
  );
}

export default App;
