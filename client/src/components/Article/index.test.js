import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import Article from './index';

it('renders correctly', () => {
    const component = shallow(<Article />);

    expect(component).toMatchSnapshot();
});
