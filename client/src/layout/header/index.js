import React, { Component } from 'react';
import { NavLink, Link } from "react-router-dom";
import ReactDOM from 'react-dom';
import Notifications from 'components/Notifications';
import ProfileMenu from 'components/ProfileMenu';
import ArticlesMenu from 'components/ArticlesMenu';
import Sidebar from 'components/Sidebar';
import MenuBar from 'components/MenuBar';
import logo from 'assets/logo.png';

import './index.css';

const modalRoot = document.getElementById('modal-root');

export default class Header extends Component {
    componentDidMount() {
        modalRoot && modalRoot.appendChild(this.el);
        this.handleResize();
        window.addEventListener('resize', this.handleResize);
    }

    componentWillUnmount() {
        modalRoot && modalRoot.removeChild(this.el);
        window.removeEventListener('resize', this.handleResize);
    }

    onHide = () => {
        this.setState({
            mobileMenuVisible: false
        });
    }

    onControlssHide = () => {
        this.setState({
            controlsVisible: false
        });
    }

    state = {
        mobileMenuVisible: false,
        controlsVisible: false,
    }

    el = document.createElement('div');

    handleResize = () => {
        window.innerWidth >= 1025
            && this.onHide()
            && this.onControlssHide();
    }

    renderSidebar() {
        return (
            ReactDOM.createPortal(
                <Sidebar
                    key="menu"
                    visible={this.state.mobileMenuVisible}
                    position='right'
                    className="mobile-menu-wrapper"
                    onHide={this.onHide}
                >
                    {this.renderMenuLinks()}
                </Sidebar>,
                this.el
            )
        )
    }

    renderMenuLinks() {
        return (
            <ul className="menu">
                <li>
                    <NavLink
                        to="/"
                        activeClassName="active"
                        exact
                    >
                        Dashboard
                    </NavLink>
                </li>
                <li>
                    <NavLink
                        to="/login"
                        activeClassName="active"
                        exact
                    >
                        Login
                    </NavLink>
                </li>
                <li>
                    <NavLink
                        to="/users"
                        activeClassName="active"
                        exact
                    >
                        Users
                    </NavLink>
                </li>
                <li>
                    <NavLink
                        to="/roles"
                        activeClassName="active"
                        exact
                    >
                        Roles
                    </NavLink>
                </li>
                <li>
                    <NavLink
                        to="/tenants"
                        activeClassName="active"
                        exact
                    >
                        Tenants
                    </NavLink>
                </li>
            </ul>
        );
    }

    renderNavButton() {
        return (
            <button
                key="nav"
                className="header-bar-button"
                onClick={() => this.setState({ mobileMenuVisible: true })}
            >
                <i className="fa far fa-navicon" />
            </button>
        );
    }

    renderControlsButton() {
        return (
            <button
                className="controls-button"
                onClick={() => this.setState({ controlsVisible: !this.state.controlsVisible })}
            >
                <i className="fa far fa-ellipsis-h" />
            </button>
        );
    }

    renderMobileButtons() {
        return (
            <div className="header-mobile-buttons">
                {this.renderNavButton()}
                {this.renderControlsButton()}
            </div>
        );
    }

  render() {
        const {
            controlsVisible,
        } = this.state;

        return (
            <header className={`header${controlsVisible ? ' controls-visible' : ''}`}>
                <div className="header-top-wrapper">
                    <div className="header-top">
                        <div className="header-left">
                            <div className="logo">
                                <Link to="/">
                                    <img src={logo} alt="" />
                                </Link>
                            </div>
                            {this.renderMobileButtons()}
                        </div>
                        <div className={`header-right${controlsVisible ? ' show-controls' : ''}`}>
                            <div className="options">
                                <div className="header-notifications">
                                    <Notifications />
                                </div>
                                <div className="header-profile">
                                    <ProfileMenu />
                                </div>
                                <div className="header-news">
                                    <ArticlesMenu />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="header__menu">
                        <MenuBar />
                    </div>
                </div>
                {this.renderSidebar()}
            </header>
        );
    }
}
