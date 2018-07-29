import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import Notifications from './index';

it('renders correctly', () => {
    const component = shallow(<Notifications />);

    expect(component).toMatchSnapshot();
});

describe('popup is showing after click on trigger button', () => {
    const component = shallow(<Notifications />);
    const button = component.find('.notification-button');

    test('popup is hidden by default', () => {
        expect(component.state().showPopup).toBe(false);
    });

    test('popup is visible after click on button', () => {
        button.simulate('click');
        expect(component.state().showPopup).toBe(true);
    });
});
