import React, { Component } from 'react';
import { Link } from "react-router-dom";
import { TransitionGroup, CSSTransition } from 'react-transition-group';
import Button from 'components/Button';
import image from './assets/user.jpg';
import './index.css';

export default class ProfileMenu extends Component {
    componentDidMount() {
        document.addEventListener('mousedown', this.handleClickOutside);
    }

    componentWillUnmount() {
        document.removeEventListener('mousedown', this.handleClickOutside);
    }

    onProfileClick = () => {
        this.toggleMenu();
    }

    setMenuRef = ref => {
        this.menuRef = ref;
    }

    setTogglerRef = ref => {
        this.togglerRef = ref;
    }

    state = {
        showMenu: false
    }

    links = [{
        icon: <i className="fa fa-user-circle" />,
        link: '',
        text: 'My profile',
        counter: true
    }, {
        icon: <i className="fa fa-superpowers" />,
        link: '',
        text: 'Activity',
    }, {
        icon: <i className="fa fa-cogs" />,
        link: '',
        text: 'My settings',
    }, {
        icon: <i className="fa fa-question-circle" />,
        link: '',
        text: 'Faq',
    }, {
        icon: <i className="fa fa-life-ring" />,
        link: '',
        text: 'Support',
    }];

    toggleMenu = (state = null) => {
        this.setState({
            showMenu: state === null
                ? !this.state.showMenu
                : state
        });
    }

    handleClickOutside = ({ target }) => {
        this.menuRef
            && !this.menuRef.contains(target)
            && !this.togglerRef.contains(target)
            && this.toggleMenu(false)
    }

    renderLinks() {
        return this.links.map(({ icon, link, text, counter }, index) => (
            <li key={index}>
                <Link className="profile-link" to={link}>
                    <div className="profile-links-icon">
                        {icon}
                    </div>
                    <div className="profile-links-text">
                        {text}
                    </div>
                    {counter && <div className="counter">2</div>}
                </Link>
            </li>
        ))
    }

    renderProfileMenu() {
        return (
            <CSSTransition
                classNames="fade"
                timeout={{ enter: 500, exit: 300 }}
            >
                <div
                    key="profile-menu"
                    className="profile-menu-wrapper"
                    ref={this.setMenuRef}
                >
                    <div className="profile-menu">
                        <div className="profile-preview">
                            <div className="profile-preview-photo">
                                <img src={image} alt="avatar" />
                            </div>
                            <div className="profile-preview-info">
                                <div
                                    className="profile-preview-info-name"
                                    title="John Doe"
                                >
                                    John Doe
                                </div>
                                <a
                                    className="profile-preview-info-email"
                                    title="johndoe@somewebsite.com"
                                >
                                    johndoe@somewebsite.com
                                </a>
                            </div>
                        </div>
                        <div className="profile-content">
                            <ul className="profile-links">
                                {this.renderLinks()}
                            </ul>
                            <Button
                                className="profile-content-logout"
                                label="Logout"
                            />
                        </div>
                    </div>
                </div>
            </CSSTransition>
        );
    }

    render() {
        const { showMenu } = this.state;

        return (
            <div className="profile">
                <button
                    onClick={this.onProfileClick}
                    className="photo-button"
                    ref={this.setTogglerRef}
                >
                    <img src={image} alt="avatar" />
                </button>
                <TransitionGroup
                >
                    {showMenu && this.renderProfileMenu()}
                </TransitionGroup>
            </div>
        );
    }
}
