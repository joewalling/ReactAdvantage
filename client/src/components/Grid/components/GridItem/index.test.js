import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import GridItem from './index';

describe('it renders correctly', () => {
    test('GridItem renders correctly without additional class names', () => {
        const component = shallow(<GridItem />);

        expect(component).toMatchSnapshot();
    });

    test('GridItem renders correctly with additional class names', () => {
        const component = shallow(<GridItem gridClassNames="ui-lg-3" />);

        expect(component).toMatchSnapshot();
    });
});
