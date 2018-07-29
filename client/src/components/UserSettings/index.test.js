import React from 'react';
import Enzyme, { shallow, mount } from 'enzyme';

import UserSettings from './index';

it('renders correctly', () => {
    const component = shallow(<UserSettings />);

    expect(component).toMatchSnapshot();
});

describe('inputs are working correctly', () => {
    const component = mount(<UserSettings onHide={() => {}} />);

    test('text input changes it\'s value', () => {
        const input = component.find('input[name="firstName"]');
        const name = input.props().name;

        input.simulate('change', {
            target: {
                name,
                value: 'test',
            },
        });

        expect(component.state().form[name].value).toBe('test');
    });

    test('file input changes it\'s value', () => {
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

test('form save button is working', () => {
    const onSave = jest.fn();
    const component = mount(
        <UserSettings
            onSave={onSave}
            onHide={() => {}}
        />
    );

    component.setState({
        form: {
            firstName: {
                value: 'test',
                validators: ['isFilled'],
                error: '',
            },
            lastName: {
                value: 'test',
                validators: ['isFilled'],
                error: '',
            },
            email: {
                value: 'test@test.com',
                validators: ['isEmail', 'isFilled'],
                error: '',
            },
            phone: {
                value: '',
            },
            name: {
                value: 'test',
            },
            password: {
                value: 'test',
            },
            repeatPassword: {
                value: 'test',
            },
            image: {
                value: '',
            },
        }
    });

    component.find('.profile-form-save').first().simulate('click');

    expect(onSave).toBeCalled();
});

