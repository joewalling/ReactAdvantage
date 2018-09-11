import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import Tag from './index';

describe('tags renders correctly', () => {
    test('default Tag renders correctly', () => {
        const component = shallow(<Tag />);

        expect(component).toMatchSnapshot();
    });

    test('announcement Tag renders correctly', () => {
        const component = shallow(<Tag type="announcement" />);

        expect(component).toMatchSnapshot();
    });

    test('enhancement Tag renders correctly', () => {
        const component = shallow(<Tag type="enhancement" />);

        expect(component).toMatchSnapshot();
    });
});
