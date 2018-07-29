import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Sidebar from 'components/Sidebar';
import Article from 'components/Article';
import './index.css';

const modalRoot = document.getElementById('modal-root');

export default class SidebarMenu extends Component {
    componentDidMount() {
        modalRoot && modalRoot.appendChild(this.el);
    }

    componentWillUnmount() {
        modalRoot && modalRoot.removeChild(this.el);
    }

    onSidebarHide = () => {
        this.toggleSidebar(false);
    }

    onSidebarButtonClick = () => {
        this.toggleSidebar();
    }

    state = {
        visible: false
    }

    articles = [{}, {}, {}, {}];

    el = document.createElement('div');

    toggleSidebar = (state = null) => {
        this.setState({
            visible: state === null
                ? !this.state.visible
                : state
        });
    }

    renderArticles() {
        return this.articles
            .map((article, index) => (
                <Article
                    key={index}
                    index={index}
                    article={article}
                />
            )
        );
    }

    render() {
        return (
            <div className="artciles-menu-wrapper">
                <button
                    onClick={this.onSidebarButtonClick}
                    className="artciles-menu-button"
                >
                    <i className="fa far fa-list-alt" />
                </button>
                {ReactDOM.createPortal(
                  <Sidebar
                        className="articles-sidebar"
                        position="right"
                        visible={this.state.visible}
                        onHide={this.onSidebarHide}
                    >
                        <div className="articles-wrapper">
                            <h2 className="sidebar-title">What's new?</h2>
                            {this.renderArticles()}
                        </div>
                    </Sidebar>,
                  this.el,
                )}

            </div>
        );
    }
}
