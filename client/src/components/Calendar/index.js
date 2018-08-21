import React, { Component } from 'react';
import { Calendar as PrimeCalendar } from 'primereact/components/calendar/Calendar';

import './index.css';

export default class Calendar extends Component {
    render() {
        return (
            <div className="calendar">
                <PrimeCalendar
                    className="ui-datepicker-trigger"
                    showIcon
                    {...this.props}
                />
            </div>
        );
    }
}
