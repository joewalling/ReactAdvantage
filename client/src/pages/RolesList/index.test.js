import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import RolesList from './index';

it('renders correctly', () => {
    const component = shallow(<RolesList />);

    expect(component).toMatchSnapshot();
});

it('render correctly with form', () => {
    const component = shallow(<RolesList />);

    component.setState({
        popupVisible: true,
    });

    expect(component).toMatchSnapshot();
});

describe('form is working correctly', () => {
    const component = shallow(<RolesList />);

    test('form is hidden by default', () => {
        expect(component.state().popupVisible).toBe(false);
    });

    test('form is visible after "onEdit" call', () => {
        component.instance().onEdit();
        expect(component.state().popupVisible).toBe(true);
    });

    test('click on "create role" is showing create role form', () => {
        const onEdit = jest.fn();
        component.onEdit = onEdit;
        const spy = jest.spyOn(component.instance(), 'onEdit');
        component.update();

        component.instance().onCreateRole();
        expect(spy).toBeCalled();
        expect(component).toMatchSnapshot();
    });
});


test('dropdown is changing entries', () => {
    const component = shallow(<RolesList />);

    component.instance().onDropdownChange({ value: 100 });

    expect(component.state().entries).toBe(100);
});

test('filters are working', () => {
    const component = shallow(<RolesList />);
    const query = '(firstName > "mock")';

    component.instance().onFilterChange({
        query,
    });

    expect(component.state().query).toBe(query);
});

test('submit is hiding form', () => {
    const component = shallow(<RolesList />);

    component.instance().onEditSubmit(null);

    expect(component.state().popupVisible).toBe(false);
    expect(component.state().selectedRole).toBe(null);
});
