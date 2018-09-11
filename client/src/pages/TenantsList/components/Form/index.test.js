import React from 'react';
import Enzyme, { shallow, mount } from 'enzyme';

import Form from './index';

const tenant = {
    tenantName: 'Default',
    name:'Default',
    edition:'Premium',
    id: '1',
    creationTime: '1529242113',
};

it('renders correctly', () => {
    const component = shallow(
        <Form tenant={tenant} />
    );

    expect(component).toMatchSnapshot();
});

test('inputs working correctly', () => {
    const component = mount(
        <Form
            tenant={{}}
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

test('form is passing values to parent component on submit', () => {
    const mockValues = {
        name: 'test',
        editions: 'Standard',
        subscription: "1529242",
        active: true,
    };

    const onSubmit = jest.fn();
    const component = mount(
        <Form
            tenant={tenant}
            onHide={() => {}}
            onSubmit={onSubmit}
        />
    );

    const form = component.state().form;
    const button = component.find('.edit-tenant-save-button').first();

    form.name.value = mockValues.name;
    form.editions.value = mockValues.editions;
    form.subscription.value = new Date(1529242000);
    form.active.value = mockValues.active;
    component.setState(form);

    button.simulate('click');

    expect(onSubmit).toHaveBeenCalledWith(mockValues);
});

test('dropdown is working', () => {
    const component = mount(
        <Form
            tenant={tenant}
            onHide={() => {}}
        />
    );

    const mockValue = 'Standard';

    component.instance().onDropdownChange({ value: mockValue });
    expect(component.state().form.editions.value).toBe(mockValue);
});

test('form is cancelling correctly', () => {
    const onHide = jest.fn();
    const component = mount(
        <Form
            tenant={tenant}
            onHide={onHide}
        />
    );

    component.find('.edit-tenant-cancel-button').first().simulate('click');
    expect(onHide).toHaveBeenCalled();
});
