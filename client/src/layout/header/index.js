import React, { Component } from 'react';
import { NavLink, Link } from "react-router-dom";
import ReactDOM from 'react-dom';
import FontAwesomeIcon from '@fortawesome/react-fontawesome';
import faBars from '@fortawesome/fontawesome-free-solid/faBars';
import faEllipsisH from '@fortawesome/fontawesome-free-solid/faEllipsisH';
import Headroom from 'react-headroom';
import Notifications from 'components/Notifications';
import ProfileMenu from 'components/ProfileMenu';
import SidebarMenu from 'components/SidebarMenu';
import Sidebar from 'components/Sidebar';
import image from './assets/logo.png';

import './index.css';

const modalRoot = document.getElementById('modal-root');

export default class Header extends Component {
    componentDidMount() {
        modalRoot.appendChild(this.el);
        this.handleResize();
        window.addEventListener('resize', this.handleResize);
        window.addEventListener('scroll', this.handleScroll);
    }

    componentWillUnmount() {
        modalRoot.removeChild(this.el);
        window.removeEventListener('resize', this.handleResize);
        window.removeEventListener('scroll', this.handleScroll);
    }

    onHide = () => this.setState({ visible: false })

    onControlssHide = () => this.setState({ controlsVisible: false })

    state = {
        visible: false,
        controlsVisible: false,
        windowWidth: '',
        scrollTop: 0
    }

    el = document.createElement('div');

    handleResize = () => {
        window.innerWidth >= 1025
            && this.onHide()
            && this.onControlssHide();

        this.setState({
            windowWidth: window.innerWidth
        });
    }

    handleScroll = () => {
        this.setState({
            scrollTop: window.scrollY,
        });
    }

    renderSidebar() {
        return (
            ReactDOM.createPortal(
                <Sidebar
                    key="menu"
                    visible={this.state.visible}
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
                        to="/menuitem1"
                        activeClassName="active"
                        exact
                    >
                        MenuItem1
                    </NavLink>
                </li>
            </ul>
        );
    }

    renderHeaderBottom() {
        const {
            scrollTop
        } = this;
        // } = this.state;

        return (
            <Headroom disableInlineStyles>
                <div className={`header-bottom${scrollTop > 100 ? ' hide' : ''}`}>
                    {this.renderMenuLinks()}
                </div>
            </Headroom>
        )
    }

    renderBarsButton() {
        return (
            <button
                key="bars"
                className="header-bar-button"
                onClick={() => this.setState({ visible: true })}
            >
                <FontAwesomeIcon className="far" icon={faBars} />
            </button>
        );
    }

    renderControlsButton() {
        return (
            <button
                className="controls-button"
                onClick={() => this.setState({ controlsVisible: !this.state.controlsVisible })}
            >
                <FontAwesomeIcon className="far" icon={faEllipsisH} />
            </button>
        );
    }

    renderMobileButtons() {
        return (
            <div className="header-mobile-buttons">
                {this.renderBarsButton()}
                {this.renderControlsButton()}
            </div>
        );
    }

    render() {
        const {
            windowWidth,
            controlsVisible,
        } = this.state;

        return (
            <header className={`header${controlsVisible ? ' controls-visible' : ''}`}>
                <div className="placeholder" />
                <div className="header-top">
                    <div className="header-left">
                        <div className="logo">
                            <Link to="/">
                                <img src={image} />
                            </Link>
                        </div>
                        {this.renderMobileButtons()}
                    </div>
                    <div className={`header-right${controlsVisible ? ' show-controls' : ''}`}>
                        <div className="options">
                            <div className="notifications">
                                <Notifications />
                            </div>
                            <div className="profile">
                                <ProfileMenu />
                            </div>
                            <div className="news">
                                <SidebarMenu />
                            </div>
                        </div>
                    </div>
                </div>

                {this.renderHeaderBottom()}
                {this.renderSidebar()}
            </header>
        );
    }
}
