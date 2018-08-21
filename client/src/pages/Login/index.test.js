import React from 'react';
import Enzyme, { shallow, mount } from 'enzyme';

import Login from './index';

it('renders correctly', () => {
    const component = shallow(<Login />);

    expect(component).toMatchSnapshot();
});

test('forgot password form renders correctly', () => {
    const component = shallow(<Login />);
    component.find('.login-forgot-password').simulate('click', {
        preventDefault: () => {},
    });

    expect(component).toMatchSnapshot();
});

test('reset password form renders correctly', () => {
    const component = shallow(<Login />);

    component.setState({
        formType: 'resetPassword',
        form: component.instance().formStates.resetPassword,
    });

    expect(component).toMatchSnapshot();
});

test('click on "forgot password" is showing forgot password form', () => {
    const component = mount(<Login />);

    component.find('.login-forgot-password').simulate('click');
    expect(component.state().formType).toBe('forgotPassword');
});

test('email submit is showing "reset password" form', () => {
    const component = mount(<Login />);

    component.setState({
        formType: 'forgotPassword',
        form: component.instance().formStates.forgotPassword,
    });

    component.setState({
        form: {
            email: {
                value: 'test@test.test',
            },
        },
    });

    component.find('.login-forgot-password').first().simulate('click');

    expect(component.state().formType).toBe('resetPassword');
});

describe('inputs are working correctly', () => {
    const component = mount(<Login />);

    test('text inputs are working correctly', () => {
        component.find('input[name="name"]').simulate('change', {
            target: {
                value: 'test',
                name: 'name',
            },
        });

        expect(component.state().form.name.value).toBe('test');
    });

    test('checkbox input is working correctly', () => {
        component.find('input[name="rememberMe"]').simulate('click');

        expect(component.state().form.rememberMe.value).toBe(true);
    });
});

test('"back" button on "forgot password" is working correctly', () => {
    const component = mount(<Login />);
    component.find('.login-forgot-password').simulate('click', {
        preventDefault: () => {},
    });
    component.find('.login-forgot-password-back-btn').first().simulate('click', {
        preventDefault: () => {},
    });

    expect(component.state().formType).toBe('login');
});
