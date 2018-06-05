import React, { Component } from 'react';
import { TransitionGroup } from 'react-transition-group';
import FadeInContainer from 'components/FadeInContainer';
import HideOnClickOutsideContainer from 'components/HideOnClickOutsideContainer';

import './index.css';

export default class Notifications extends Component {
    onNotificationsClick = () => {
        this.togglePopup();
    }

    setTogglerRef = ref => {
        this.togglerRef = ref;
    }

    state = {
        showPopup: false,
    }

    togglePopup = (state = null) => {
        this.setState({
            showPopup: state === null
                ? !this.state.showPopup
                : state
        });
    }

    renderNotifications() {
        return (
            <FadeInContainer>
                <div className="notifications-popup">
                    <div className="notifications-popup-header">
                        <div className="notifications-popup-header-title">
                            9 New
                        </div>
                        <div className="notifications-popup-header-subtitle">
                            User notifications
                        </div>
                    </div>
                </div>
            </FadeInContainer>
        )
    }

    render() {
        const { showPopup } = this.state;

        return (
            <HideOnClickOutsideContainer
                onHide={() => this.togglePopup(false)}
            >
                <div className="notifications">
                    <button
                        onClick={this.onNotificationsClick}
                        className="notification-button"
                    >
                        <i className="fa far fa-bell" />
                    </button>
                    <div className="notifications-animation-wrapper">
                        <TransitionGroup>
                            {showPopup && this.renderNotifications()}
                        </TransitionGroup>
                    </div>
                </div>
            </HideOnClickOutsideContainer>
        );
    }
}
