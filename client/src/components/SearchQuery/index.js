import React, { Component } from 'react';
import TwoWayQuerybuilder from 'react-two-way-querybuilder-ra';

import Button from 'components/Button';
import Dropdown from 'components/Dropdown';
import Calendar from 'components/Calendar';

import './index.css';

export default class SearchQuery extends Component {
    constructor(props) {
        super(props);
        this.config = {
            styles: {
                primaryBtn: 'ui-corner-all ui-widget ui-button ui-state-default',
                deleteBtn: 'ui-corner-all ui-widget ui-button ui-button-danger ui-state-default',
                rule: 'rule',
                condition: 'condition',
                select: 'querySelect',
                input: 'ui-inputtext ui-state-default ui-corner-all ui-widget',
                txtArea: 'queryText',
            },
            operators: [{
                operator: '=',
                label: '='
            }, {
                operator: '<>',
                label: '<>'
            }, {
                operator: '<',
                label: '<'
            }, {
                operator: '>',
                label: '>'
            }, {
                operator: '>=',
                label: '>='
            }, {
                operator: '<=',
                label: '<='
            }, {
                operator: 'IS NULL',
                label: 'Null'
            }, {
                operator: 'IS NOT NULL',
                label: 'Not Null'
            }, {
                operator: 'IN',
                label: 'Contains'
            }, {
                operator: 'NOT IN',
                label: 'Does Not Contain',
            }, {
                operator: 'LIKE',
                label: 'Starts With',
            }],
            selectRenderer: this.renderSelect,
            datepickerRenderer: this.renderDatepicker,
        };
    }

    renderSelect = ({
        options,
        value,
        className,
        onChange,
    }) => (
        <Dropdown
            options={options}
            value={value}
            className="filters-dropdown"
            onChange={({ value }) => {
                onChange(value);
            }}
        />
    )

    renderDatepicker = ({ value, onChange }) => (
        <Calendar
            showTime
            className="filters-datepicker"
            value={value}
            onChange={({ originalEvent, value }) => onChange(value)}
        />
    )

    renderButton() {
        const {
            onSearch,
            searchLabel = 'Search',
        } = this.props;

        return (
            <div className="filters-search-button-wrapper">
                <Button label={searchLabel} onClick={onSearch}></Button>
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
