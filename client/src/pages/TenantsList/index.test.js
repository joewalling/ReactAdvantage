import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import TenantsList from './index';

it('renders correctly', () => {
    const component = shallow(<TenantsList />);

    expect(component).toMatchSnapshot();
});

it('render correctly with form', () => {
    const component = shallow(<TenantsList />);

    component.setState({
        popupVisible: true,
    });

    expect(component).toMatchSnapshot();
});

describe('form is working correctly', () => {
    const component = shallow(<TenantsList />);

    test('form is hidden by default', () => {
        expect(component.state().popupVisible).toBe(false);
    });

    test('form is visible after "onEdit" call', () => {
        component.instance().onEdit();
        expect(component.state().popupVisible).toBe(true);
    });

    test('click on "create tenant" is showing create tenant form', () => {
        const onEdit = jest.fn();
        component.onEdit = onEdit;
        const spy = jest.spyOn(component.instance(), 'onEdit');
        component.update();

        component.instance().onCreateTenant();
        expect(spy).toBeCalled();
        expect(component).toMatchSnapshot();
    });
});


test('dropdown is changing entries', () => {
    const component = shallow(<TenantsList />);

    component.instance().onDropdownChange({ value: 100 });

    expect(component.state().entries).toBe(100);
});

test('filters are working', () => {
    const component = shallow(<TenantsList />);
    const query = '(firstName > "mock")';

    component.instance().onFilterChange({
        query,
    });

    expect(component.state().query).toBe(query);
});

test('submit is hiding form', () => {
    const component = shallow(<TenantsList />);

    component.instance().onEditSubmit(null);

    expect(component.state().popupVisible).toBe(false);
    expect(component.state().selectedUser).toBe(null);
});
