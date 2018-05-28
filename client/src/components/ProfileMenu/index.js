import React, { Component } from 'react';
import { Link } from "react-router-dom";
import ReactCSSTransitionGroup from 'react-addons-css-transition-group'; // ES6
import FontAwesomeIcon from '@fortawesome/react-fontawesome';
import faBell from '@fortawesome/fontawesome-free-solid/faBell';
import faUserCircle from '@fortawesome/fontawesome-free-solid/faUserCircle';
import faSyncAlt from '@fortawesome/fontawesome-free-solid/faSyncAlt';
import faCogs from '@fortawesome/fontawesome-free-solid/faCogs';
import faQuestionCircle from '@fortawesome/fontawesome-free-solid/faQuestionCircle';
import faLifeRing from '@fortawesome/fontawesome-free-solid/faLifeRing';
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
        icon: <FontAwesomeIcon className="far" icon={faUserCircle} />,
        link: '',
        text: 'My profile',
        counter: true
    }, {
        icon: <FontAwesomeIcon className="far" icon={faSyncAlt} />,
        link: '',
        text: 'Activity',
    }, {
        icon: <FontAwesomeIcon className="far" icon={faCogs} />,
        link: '',
        text: 'My settings',
    }, {
        icon: <FontAwesomeIcon className="far" icon={faQuestionCircle} />,
        link: '',
        text: 'Faq',
    }, {
        icon: <FontAwesomeIcon className="far" icon={faLifeRing} />,
        link: '',
        text: 'Support',
    }];

    toggleMenu = (state = null) => {
        this.setState({
            showMenu: state === null
                ? !this.state.showMenu
                : state
        })
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
            <div
                key="profile-menu"
                className="profile-menu-wrapper"
                ref={this.setMenuRef}
            >
                <div className="profile-menu">
                    <div className="profile-preview">
                        <div className="profile-preview-photo">
                            <img src={image}/>
                        </div>
                        <div className="profile-preview-info">
                            <div className="profile-preview-info-name">
                                Mark Andre
                            </div>
                            <a className="profile-preview-info-email">
                                forcu2mblog@gmail.com
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
        );
    }

    render() {
        const { showMenu } = this.state;

        return (
            <div className="profile">
                <div
                    onClick={() => this.toggleMenu()}
                    className="photo"
                    ref={this.setTogglerRef}
                >
                    <img src={image} />
                </div>
                <ReactCSSTransitionGroup
                    transitionName="example"
                    transitionEnterTimeout={500}
                    transitionLeaveTimeout={300}
                >
                    {showMenu && this.renderProfileMenu()}
                </ReactCSSTransitionGroup>
            </div>
        );
    }
}
