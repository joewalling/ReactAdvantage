import React, { Component } from 'react';
import TwoWayQuerybuilder from 'react-two-way-querybuilder';

import Button from 'components/Button';

import './index.css';

export default class SearchQuery extends Component {
    config = {
        styles: {
            primaryBtn: 'ui-button ui-state-default',
            deleteBtn: 'ui-button ui-button-danger ui-state-default',
            rule: 'rule',
            condition: 'condition',
            select: 'querySelect',
            input: 'ui-inputtext ui-state-default',
            txtArea: 'queryText',
        }
    }

    renderButton() {
        const {
            onSearch,
            searchLabel = 'Search',
        } = this.props;

        return (
            <div className="filters-search-button-wrapper">
                <Button onClick={onSearch}>
                    {searchLabel}
                </Button>
            </div>
        );
    }

    render() {
        const {
            title = 'Filters',
            onSearch,
        } = this.props;

        return (
            <div className="filters-wrapper">
                <div className="filters">
                    <div className="search-query-wrapper">
                        {title && (
                            <div className="search-query-title">{title}</div>
                        )}
                        <div className="search-query">
                            <TwoWayQuerybuilder
                                fields={this.props.fields}
                                onChange={this.props.onFilterChange}
                                config={this.config}
                                {...this.props}
                            />
                        </div>
                    </div>
                </div>
                {onSearch && this.renderButton()}
            </div>
        );
    }
}
