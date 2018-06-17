import React, { Component } from 'react';
import { Link } from "react-router-dom";
import { TransitionGroup } from 'react-transition-group';
import Button from 'components/Button';
import FadeInContainer from 'components/FadeInContainer';
import HideOnClickOutsideContainer from 'components/HideOnClickOutsideContainer';
import UserSettings from 'components/UserSettings';
import image from './assets/user.jpg';
import './index.css';

export default class ProfileMenu extends Component {
    onProfileClick = () => {
        this.toggleMenu();
    }

    onSettingsClick = event => {
        event.preventDefault();

        this.setState({
            showSettings: true,
            showMenu: false,
        });
    }

    onSettingsHide = () => {
        this.setState({
            showSettings: false,
        });
    }

    onSettingsSave = formValues => {
        console.log('Success! Form values will be bellow');
        console.log(formValues);
        this.onSettingsHide();
    }

    state = {
        showMenu: false,
        showSettings: false,
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
        onClick: this.onSettingsClick,
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

    renderLinks() {
        return this.links.map(({
            icon,
            link,
            text,
            counter,
            onClick
        }, index) => (
            <li key={index}>
                <Link
                    className="profile-link"
                    to={link}
                    onClick={onClick}
                >
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
            <FadeInContainer>
                <div
                    key="profile-menu"
                    className="profile-menu-wrapper"
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
            </FadeInContainer>
        );
    }

    render() {
        const { showMenu } = this.state;

        return (
            <HideOnClickOutsideContainer
                onHide={() => this.toggleMenu(false)}
            >
                <div className="profile">
                    <button
                        onClick={this.onProfileClick}
                        className="photo-button"
                    >
                        <img src={image} alt="avatar" />
                    </button>
                    <div className="profile-menu-animation">
                        <TransitionGroup>
                            {showMenu && this.renderProfileMenu()}
                        </TransitionGroup>
                    </div>
                </div>
                <UserSettings
                    visible={this.state.showSettings}
                    onSave={this.onSettingsSave}
                    onHide={this.onSettingsHide}
                />
            </HideOnClickOutsideContainer>
        );
    }
}
