
import { useDispatch, useSelector } from 'react-redux'
import { FaGlobe, FaUser } from 'react-icons/fa';
import {
    NavLink
} from 'react-router-dom';

import Translation from './Translation';
import { setLang } from '../store/reducers/userReducer';
import { clearToken } from '../store/reducers/authReducer';

function Navbar() {
    const dispatch = useDispatch()

    const isLoggedIn = useSelector((state) => state.auth.isLoggedIn)

    function LoginButton() {
        return (
            <NavLink
                to="/login"
                className={"nav-link"}
                style={{ color: '#0275d8' }} // bootstrap primary color
            ><Translation msg="LOGIN" /></NavLink>
        )
    }

    function LogoutButton() {
        return (
            <NavLink
                to="/"
                className={"nav-link"}
                onClick={() => logout()}
                style={{ color: '#d9534f' }} // bootstrap danger color
            ><Translation msg="LOGOUT" /></NavLink>
        )
    }

    function logout() {
        dispatch(clearToken())
    }

    return (
        <header className="navbar navbar-expand-lg bg-light card mb-3">
            <nav className="container-xxl bd-gutter flex-wrap flex-lg-nowrap">
                <a className="navbar-brand" href="/"><Translation msg="APP_NAME" /></a>
                <button className="navbar-toggler"
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#navbarSupportedContent"
                    aria-controls="navbarSupportedContent"
                    aria-expanded="false"
                    aria-label="Toggle navigation">
                    <span className="navbar-toggler-icon"></span>
                </button>
                <div className="collapse navbar-collapse" id="navbarSupportedContent">
                    <ul className="navbar-nav me-auto mb-2 mb-lg-0">
                        {
                            isLoggedIn ?
                                <>
                                    <li className="nav-item">
                                        <NavLink
                                            to="/password"
                                            className={"nav-link"}
                                            style={{ marginLeft: "5px" }}
                                        ><Translation msg="PASSWORDS" /></NavLink>
                                    </li>
                                    <li className="nav-item">
                                        <NavLink
                                            to="/note"
                                            className={"nav-link"}
                                            style={{ marginLeft: "5px" }}
                                        ><Translation msg="NOTES" /></NavLink>
                                    </li>
                                    <li className="nav-item">
                                        <NavLink
                                            to="/tool"
                                            className={"nav-link"}
                                            style={{ marginLeft: "5px" }}
                                        ><Translation msg="TOOLS" /></NavLink>
                                    </li>
                                </>
                                : null
                        }
                    </ul>
                    <div className='navbar-nav'>
                        <div className="dropdown">
                            <div className="btn btn-link dropdown-toggle" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                                <FaGlobe size="22" color='grey' />
                            </div>
                            <ul className="dropdown-menu dropdown-menu-end">
                                <li><button className="dropdown-item" onClick={() => { dispatch(setLang('TR')) }}>TR</button></li>
                                <li><button className="dropdown-item" onClick={() => { dispatch(setLang('ENG')) }}>ENG</button></li>
                            </ul>
                        </div>
                        {
                            isLoggedIn ?
                                <div className="dropdown">
                                    <div className="btn btn-link dropdown-toggle" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                                        <FaUser size="22" color='grey' />
                                    </div>
                                    <ul className="dropdown-menu dropdown-menu-end">
                                        <li>
                                            <NavLink
                                                to="/profile"
                                                className={"nav-link"}
                                                style={{ marginLeft: "5px" }}
                                            ><Translation msg="PROFILE" />
                                            </NavLink>
                                        </li>
                                        <li><hr className="dropdown-divider" /></li>
                                        <li style={{ textAlign: "center" }}>
                                            <LogoutButton />
                                        </li>
                                    </ul>
                                </div>
                                : <LoginButton />
                        }
                    </div>
                </div>

            </nav>
        </header>
    )
}

export default Navbar