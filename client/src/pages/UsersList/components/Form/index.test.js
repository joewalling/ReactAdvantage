import React from 'react';
import Enzyme, { shallow, mount, render } from 'enzyme';

import ModalPopup from 'components/ModalPopup';

import Form from './index';

const user = {
    firstName: 'test',
    lastName: 'test',
    email: 'test@test.test',
    name: 'test',
};

test('create form renders correctly', () => {
    const component = shallow(<Form user={{}} />);

    expect(component).toMatchSnapshot();
});

test('form with active roles tab renders correctly', () => {
    const component = shallow(<Form user={{}} />);
    component.instance().onTabChange({ index: 1 });

    expect(component).toMatchSnapshot();
});

test('edit form renders correctly', () => {
    const component = shallow(<Form user={user} />);

    expect(component).toMatchSnapshot();
});

describe('inputs are working correctly', () => {
    const component = mount(
        <Form
            user={{}}
            onHide={() => {}}
        />
    );

    test('text input is working', () => {
        const name = 'firstName';
        const input = component.find(`input[name="${name}"]`);

        input.simulate('change', {
            target: {
                name,
                value: 'test',
            },
        });

        expect(component.state().form[name].value).toBe('test');
    });

    test('file input is working', () => {
        const input = component.find('input[type="file"]');
        const file = new Blob(['file content'], { type : 'text/plain' });

        input.simulate('change', {
            target: {
                files: [
                    file,
                ]
            },
        });

        expect(component.state().form.image.value).toBe(file);
    });
});

describe('password fields is hidden after click on "Set random password" checkbox', () => {
    const component = mount(
        <Form
            user={{}}
            onHide={() => {}}
        />
    );

    test('password fields are visible by default', () => {
        expect(component.state().form.randomPassword.value).toBe(false);
    });

    test('password fields are hidden after click on "Set random password" checkbox', () => {
        const name = 'randomPassword';
        const checkbox = component.find(`input[name="${name}"]`);

        checkbox.simulate('click', {
            target: {
                name,
                value: true,
            },
        });

        expect(component.state().form.randomPassword.value).toBe(true)
    });

});

test('form with random password renders correctly', () => {
    const component = shallow(
        <Form
            user={user}
            onHide={() => {}}
        />
    );
    const form = component.state().form;

    form.randomPassword.value = false;

    component.setState({
        form,
    });

    expect(component).toMatchSnapshot();
});

test('tab is changing correctly', () => {
    const component = shallow(
        <Form
            user={user}
            onHide={() => {}}
        />
    );

    component.instance().onTabChange({ index: 1 });

    expect(component.state().activeTabIndex).toBe(1);
});

test('form is passing values to parent component on submit', () => {
    const onSubmit = jest.fn();
    const component = mount(
        <Form
            user={user}
            onHide={() => {}}
            onSubmit={onSubmit}
        />
    );

    const form = component.state().form;
    const button = component.find('.edit-user-save-button').first();

    form.randomPassword.value = true;
    component.setState(form)

    button.simulate('click');

    const mockValues = {
        'activationEmail': false,
        'active': false,
        'changePasswordOnLogin': false,
        'email': 'test@test.test',
        'firstName': 'test',
        'image': '',
        'lastName': 'test',
        'name': 'test',
        'phone': '',
        'randomPassword': true,
        'roles': [],
    };

    expect(onSubmit).toHaveBeenCalledWith(mockValues);
});

test('form is cancelling correctly', () => {
    const onHide = jest.fn();
    const component = mount(
        <Form
            user={user}
            onHide={onHide}
        />
    );

    component.find('.edit-user-cancel-button').first().simulate('click');
    expect(onHide).toHaveBeenCalled();
});
