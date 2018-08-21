import React from 'react';
import Enzyme, { shallow, mount } from 'enzyme';

import Form from './index';

const role = {
    roleName: 'admin',
    id: '1',
    creationTime: '1529242113',
};

it('renders correctly', () => {
    const component = shallow(
        <Form role={role} />
    );

    expect(component).toMatchSnapshot();
});

test('form with active roles properties renders correctly', () => {
    const component = shallow(<Form role={{}} />);
    component.instance().onTabChange({ index: 1 });

    expect(component).toMatchSnapshot();
});

test('edit form renders correctly', () => {
    const component = shallow(<Form role={role} />);

    expect(component).toMatchSnapshot();
});

test('input is working correctly', () => {
    const component = mount(
        <Form
            role={{}}
            onHide={() => {}}
        />
    );

    const name = 'name';
    const input = component.find(`input[name="${name}"]`);

    input.simulate('change', {
        target: {
            name,
            value: 'test',
        },
    });

    expect(component.state().form[name].value).toBe('test');
});

test('tabs are changing correctly', () => {
    const component = shallow(
        <Form
            role={role}
            onHide={() => {}}
        />
    );

    component.instance().onTabChange({ index: 1 });

    expect(component.state().activeTabIndex).toBe(1);
});

test('form is passing values to parent component on submit', () => {
    const mockValues = {
        name: 'mock',
        default: true,
        permissions: ['audit-logs', 'email-templates'],
    };

    const onSubmit = jest.fn();
    const component = mount(
        <Form
            role={role}
            onHide={() => {}}
            onSubmit={onSubmit}
        />
    );

    const form = component.state().form;
    const button = component.find('.edit-role-save-button').first();
    const treeValue = mockValues.permissions;

    form.name.value = mockValues.name;
    form.default.value = mockValues.default;

    component.setState({
        treeValue,
        form,
    });

    button.simulate('click');

    expect(onSubmit).toHaveBeenCalledWith(mockValues);
});

test('form is cancelling correctly', () => {
    const onHide = jest.fn();
    const component = mount(
        <Form
            role={role}
            onHide={onHide}
        />
    );

    component.find('.edit-role-cancel-button').first().simulate('click');
    expect(onHide).toHaveBeenCalled();
});
